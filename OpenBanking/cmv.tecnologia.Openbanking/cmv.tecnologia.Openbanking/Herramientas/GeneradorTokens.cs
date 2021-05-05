using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace cmv.tecnologia.Openbanking.Herramientas
{
    public static class GeneradorTokens
    {
        public static dynamic GenerarToken(string Usuario)
        {
            //Obtenemos configuracion de JWT

            var secretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];
            var audienceToken = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
            var issuerToken = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];
            var expireTime = ConfigurationManager.AppSettings["JWT_EXPIRE_MINUTES"];


            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
            var signingcredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //creamos calins token
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] {
                new  Claim(ClaimTypes.Name, Usuario)
            });

            DateTime now = DateTime.UtcNow;
            DateTime expire = now.AddMinutes(Convert.ToInt32(expireTime));

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                audience: audienceToken,
                issuer: issuerToken,
                subject :claimsIdentity,
                notBefore: now,
                expires :expire,
                signingCredentials: signingcredentials

                );

            var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);
            string jwtTokenEncript = Encryptor.EncryptString(secretKey, jwtTokenString);
            return new
            {
                token = jwtTokenEncript,
                FechaCaducidad = expire
            };

        }
    }
}