using cmv.tecnologia.Entidades;
using cmv.tecnologia.Entidades.Catalogos;
using cmv.tecnologia.Entidades.Herramientas;
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

namespace cmv.tecnologia.NotificationService.Interceptors {
  public class SecurityInterceptor : DelegatingHandler {
    private static bool ObtenerCabeceraToken(HttpRequestMessage request, out string token) {
      token = null;
      IEnumerable<string> authzHeaders;
      if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1) {
        return false;
      }
      var bearerToken = authzHeaders.ElementAt(0);
      token = Encryptor.DecryptString(ConfigurationManager.AppSettings["JWT_SECRET_KEY"], bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken);
      return true;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
      HttpStatusCode statusCode;
      HttpContent content;
      string token;

      // determine whether a jwt exists or not
      if (!ObtenerCabeceraToken(request, out token)) {
        statusCode = HttpStatusCode.Unauthorized;
        return base.SendAsync(request, cancellationToken);
      }

      try {
        var secretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];
        var audienceToken = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
        var issuerToken = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];
        var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));

        SecurityToken securityToken;
        var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        TokenValidationParameters validationParameters = new TokenValidationParameters() {
          ValidAudience = audienceToken,
          ValidIssuer = issuerToken,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          LifetimeValidator = this.ValidadorVigencia,
          IssuerSigningKey = securityKey
        };

        // Extract and assign Current Principal and user
        Thread.CurrentPrincipal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
        HttpContext.Current.User = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
        request.Properties.Add("User", ((JwtSecurityToken)securityToken).Payload["unique_name"].ToString());
        request.Properties.Add("Password", ((JwtSecurityToken)securityToken).Payload["certpublickey"].ToString());
        return base.SendAsync(request, cancellationToken);
      } catch (SecurityTokenValidationException) {
        statusCode = HttpStatusCode.Unauthorized;
      } catch (Exception) {
        statusCode = HttpStatusCode.InternalServerError;
      }

      return Task<HttpResponseMessage>.Factory.StartNew(() => {
        var response = new HttpResponseMessage(statusCode);
        response.Content = new ObjectContent<WsRespuesta<string>>(HerramientaRespuestas<string>.CrearRespuesta(null,
          (statusCode == HttpStatusCode.Unauthorized) ? CatalogoRespuestas.NO_AUTORIZADO.Codigo : CatalogoRespuestas.ERROR_CORE.Codigo,
          (statusCode == HttpStatusCode.Unauthorized) ? CatalogoRespuestas.NO_AUTORIZADO.Mensaje : CatalogoRespuestas.ERROR_CORE.Mensaje,
          false),
          new JsonMediaTypeFormatter(), "application/json");
        return response;
      });
    }

    public bool ValidadorVigencia(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters) {
      if (expires != null) {
        if (DateTime.UtcNow < expires) return true;
      }
      return false;
    }
  }
}