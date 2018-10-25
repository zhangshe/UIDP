using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using UIDP.Model;

namespace UIDP.WebAPI
{
    /// <summary>
    /// 配置的服务和应用程序的请求管道
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 应用程序启动
        /// </summary>
        /// <param name="env"></param>
        ///// <param name="configuration"></param>
        //public Startup(IConfiguration configuration)
        public Startup(IHostingEnvironment env)
        {
            //Configuration = configuration;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            this.Configuration = builder.Build();
            var test = Configuration.ToString();
            BaseConfigModel.SetBaseConfig(Configuration, env.ContentRootPath, env.WebRootPath);
        }

        /// <summary>
        /// 配置信息
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 服务注册配置应用程序的服务This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
            #region 添加Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1.1.0",
                    Title = "UIDP WebAPI",
                    //Description = "框架集合",
                    //TermsOfService = "None",
                    //Contact = new Swashbuckle.AspNetCore.Swagger.Contact { Name = "Maverick", Email = "1312719913@qq.com", Url = "http://www.cnblogs.com/" }
                });
                //添加读取注释服务
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var apiXmlPath = Path.Combine(basePath, "UIDP.WebAPI.xml");
                c.IncludeXmlComments(apiXmlPath, true);
                var entityXmlPath = Path.Combine(basePath, "UIDP.Entity.xml");
                c.IncludeXmlComments(entityXmlPath, true);//控制器层注释（true表示显示控制器注释）
                //添加对控制器的标签(描述)
                //c.DocumentFilter<SwaggerDocTag>();
                //添加header验证信息
                //c.OperationFilter<SwaggerHeader>();
                var security = new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } }, };
                c.AddSecurityRequirement(security);//添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称要一致，这里是Bearer。
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 参数结构: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });
            });
            #endregion

            #region 认证
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    JwtAuthConfigModel jwtConfig = new JwtAuthConfigModel();
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = "UIDP",
                        ValidAudience = "wr",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.JWTSecretKey)),

                        /***********************************TokenValidationParameters的参数默认值***********************************/
                        RequireSignedTokens = true,
                        // SaveSigninToken = false,
                        // ValidateActor = false,
                        // 将下面两个参数设置为false，可以不验证Issuer和Audience，但是不建议这样做。
                        ValidateAudience = false,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        // 是否要求Token的Claims中必须包含 Expires
                        RequireExpirationTime = true,
                        // 允许的服务器时间偏移量
                        // ClockSkew = TimeSpan.FromSeconds(300),
                        // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                        ValidateLifetime = true
                    };
                });
            #endregion

            #region 授权
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireClient", policy => policy.RequireRole("Client").Build());
                options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin").Build());
                options.AddPolicy("RequireAdminOrClient", policy => policy.RequireRole("Admin,Client").Build());
            });
            #endregion

            #region 跨域CORS
            services.AddCors(c =>
            {
                c.AddPolicy("Any", policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
                c.AddPolicy("AllowSpecificOrigin", policy =>
                {
                    policy.WithOrigins("http://localhost:8083")//运行跨越访问的请求地址么,有多个的话用逗号隔开
                    .WithMethods("GET", "POST", "PUT", "DELETE")
                    .WithHeaders("authorization");
                });
            });
            #endregion

        }

        /// <summary>
        /// 创建应用程序的请求处理管道This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin();
            });

            #region 添加Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
            });
            #endregion

            #region 认证
            app.UseAuthentication();
            #endregion

            #region 授权
            app.UseMiddleware<JwtAuthorizationFilter>();
            #endregion

            app.UseMvc();

            #region 静态资源
            app.UseStaticFiles();//用于访问wwwroot下的文件 
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
            //    System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "ExcelModel")),
            //    RequestPath = "/ExcelModel"
            //});
            #endregion

            #region 解决Ubuntu Nginx 代理不能获取IP问题
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
            });
            #endregion

        }
    }
}
