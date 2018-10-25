using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using UIDP.Model;

namespace UIDP.Utility
{
    public class JwtHelper
    {
        /// <summary>
        /// 颁发JWT字符串
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public static string IssueJWT(TokenModel tokenModel)
        {
            var dateTime = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti,tokenModel.Uid.ToString()),//用户Id
                new Claim("Role", tokenModel.Role),//身份
                new Claim("Domain", tokenModel.Domain),//范围
                new Claim("TokenType", tokenModel.TokenType),//范围
                new Claim(JwtRegisteredClaimNames.Iat,dateTime.ToString(),ClaimValueTypes.Integer64)
            };
            //秘钥
            var jwtConfig = new JwtAuthConfigModel();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.JWTSecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //过期时间
            double exp = 0;
            switch (tokenModel.TokenType)
            {
                case "Web":
                    exp = jwtConfig.WebExp;
                    break;
                case "App":
                    exp = jwtConfig.AppExp;
                    break;
                case "MiniProgram":
                    exp = jwtConfig.MiniProgramExp;
                    break;
                case "Other":
                    exp = jwtConfig.OtherExp;
                    break;
            }
            var jwt = new JwtSecurityToken(
                issuer: "UIDP",
                claims: claims, //声明集合
                expires: dateTime.AddMinutes(exp),
                signingCredentials: creds);

            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static TokenModel SerializeJWT(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
            object role = new object(); ;
            object domain = new object();
            object tokentype = new object();
            try
            {
                jwtToken.Payload.TryGetValue("Role", out role);
                jwtToken.Payload.TryGetValue("Domain", out domain);
                jwtToken.Payload.TryGetValue("TokenType", out tokentype);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            var tm = new TokenModel
            {
                Uid = long.Parse(jwtToken.Id),
                Role = role.ToString(),
                Domain = domain.ToString(),
                TokenType= tokentype.ToString()
            };
            return tm;
        }
    }

    /// <summary>
    /// 令牌
    /// </summary>
    public class TokenModel
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long Uid { get; set; }
        /// <summary>
        /// 身份(角色)
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// 范围
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// 令牌类型
        /// </summary>
        public string TokenType { get; set; }
    }
}
