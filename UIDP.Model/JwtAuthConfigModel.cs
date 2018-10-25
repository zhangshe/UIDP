
using System;


namespace UIDP.Model
{
    /// <summary>
    /// Jwt验证数据模型
    /// </summary>
    public class JwtAuthConfigModel:BaseConfigModel
    {
        /// <summary>
        /// Jwt验证参数配置
        /// </summary>
        public JwtAuthConfigModel()
        {
            try
            {
                JWTSecretKey = Configuration["JwtAuth:SecurityKey"];
                WebExp = double.Parse(Configuration["JwtAuth:WebExp"]);
                AppExp = double.Parse(Configuration["JwtAuth:AppExp"]);
                MiniProgramExp = double.Parse(Configuration["JwtAuth:MiniProgramExp"]);
                OtherExp = double.Parse(Configuration["JwtAuth:OtherExp"]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        /// <summary>
        /// 加密秘钥
        /// </summary>
        public string JWTSecretKey = "This is JWT Secret Key For UIDP";//VGhpcyBpcyBKV1QgU2VjcmV0IEtleSBGb3IgVUlEUA==
        /// <summary>
        /// web应用有效期
        /// </summary>
        public double WebExp = 15;
        /// <summary>
        /// App有效期
        /// </summary>
        public double AppExp = 15;
        /// <summary>
        /// 小程序有效期
        /// </summary>
        public double MiniProgramExp = 15;
        /// <summary>
        /// 其它有效期
        /// </summary>
        public double OtherExp = 15;
    }
}
