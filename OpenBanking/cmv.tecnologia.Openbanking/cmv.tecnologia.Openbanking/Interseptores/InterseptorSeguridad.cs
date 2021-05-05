using cmv.tecnologia.Openbanking.Herramientas;
using cmv.tecnologia.Entidades;
using cmv.tecnologia.Entidades.Catalogos;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace cmv.tecnologia.Openbanking.Interseptores
{
    public class InterseptorSeguridad : DelegatingHandler
    {

        private static bool ObtenerCabeceraToken(HttpRequestMessage request, out string token)
        {
            token = null;
            if (!request.Headers.TryGetValues("Authorization",out IEnumerable<string> authHeaders) || authHeaders.Count()>1)
            {
                return false;
            }
            var bearerToken = authHeaders.ElementAt(0);
            token = Encryptor.DecryptString(ConfigurationManager.AppSettings["JWT_SECRET_KEY"], bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken);
            return true;

        }

        /// <summary>
        /// Método para interceptar las peticiones
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpStatusCode statusCode;
            // determine whether a jwt exists or not
            if (!ObtenerCabeceraToken(request, out string token))
            {
                statusCode = HttpStatusCode.Unauthorized;
                return base.SendAsync(request, cancellationToken);
            }
            try
            {
                var secretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];
                var audienceToken = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
                var issuerToken = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];
                var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
                var tokenHandler = new JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = audienceToken,
                    ValidIssuer = issuerToken,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = this.ValidadorVigencia,
                    IssuerSigningKey = securityKey

                };
                // Extract and assign Current Principal and user
                Thread.CurrentPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
                HttpContext.Current.User = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
                return base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenValidationException e)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            catch (Exception)
            {
                statusCode = HttpStatusCode.InternalServerError;
            }
            return Task<HttpResponseMessage>.Factory.StartNew(() => {
                var response = new HttpResponseMessage(statusCode);
                response.Content = new ObjectContent<WsRespuesta<string>>(ConstructorRespuesta<string>.CrearRespuesta(null,
                  (statusCode == HttpStatusCode.Unauthorized) ? CatalogoRespuestas.NO_AUTORIZADO.Codigo : CatalogoRespuestas.ERROR_INTERNO.Codigo,
                  (statusCode == HttpStatusCode.Unauthorized) ? CatalogoRespuestas.NO_AUTORIZADO.Mensaje : CatalogoRespuestas.ERROR_INTERNO.Mensaje),
                  new JsonMediaTypeFormatter(), "application/json");
                return response;
            });
        }
        /// <summary>
        /// Método para validar la vigencia del token
        /// </summary>
        /// <param name="notBefore"></param>
        /// <param name="expires"></param>
        /// <param name="securityToken"></param>
        /// <param name="validationParameters"></param>
        /// <returns></returns>
        public bool ValidadorVigencia(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
                return DateTime.UtcNow < expires;
            return false;
        }

    }
}