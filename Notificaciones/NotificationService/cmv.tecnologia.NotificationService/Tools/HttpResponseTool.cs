using cmv.tecnologia.Entidades;
using cmv.tecnologia.Entidades.Catalogos;
using cmv.tecnologia.Entidades.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;

namespace cmv.tecnologia.NotificationService.Tools {
  public class HttpResponseTool<T> {
    /// <summary>
    /// Crea excepción http
    /// </summary>
    /// <param name="codigo"></param>
    /// <param name="entidad"></param>
    /// <returns></returns>
    public static HttpResponseException BuildHttpException(HttpStatusCode codigo, T entidad) {
      return new HttpResponseException(new HttpResponseMessage(codigo) {
        Content = new ObjectContent<T>(entidad, new JsonMediaTypeFormatter(), "application/json")
      });
    }

    /// <summary>
    /// Crea una respuesta de no autorizado
    /// </summary>
    /// <returns></returns>
    public static HttpResponseException BuildHttpUnauthorizedException() {
      return new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized) {
        Content = new ObjectContent<WsRespuesta<string>>(
          HerramientaRespuestas<string>.CrearRespuesta(
            null, CatalogoRespuestas.NO_AUTORIZADO.Codigo, CatalogoRespuestas.NO_AUTORIZADO.Mensaje, false),
          new JsonMediaTypeFormatter(), "application/json")
      });
    }

    /// <summary>
    /// Crea una respuesta de error
    /// </summary>
    /// <returns></returns>
    public static HttpResponseException BuildHttpErrorException(Exception e) {
      return new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) {
        Content = new ObjectContent<WsRespuesta<Exception>>(
          HerramientaRespuestas<Exception>.CrearRespuestaErronea(e),
          new JsonMediaTypeFormatter(), "application/json")
      });
    }

    /// <summary>
    /// Crea una respuesta de solicitud erronea
    /// </summary>
    /// <returns></returns>
    public static HttpResponseException BuildHttpBadRequestException() {
      return new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) {
        Content = new ObjectContent<WsRespuesta<string>>(
          HerramientaRespuestas<string>.CrearRespuesta(
            null, CatalogoRespuestas.SOLICITUD_INCORRECTA.Codigo, CatalogoRespuestas.SOLICITUD_INCORRECTA.Mensaje, false),
          new JsonMediaTypeFormatter(), "application/json")
      });
    }

    /// <summary>
    /// Crea una respuesta de solicitud erronea
    /// </summary>
    /// <returns></returns>
    public static HttpResponseException BuildHttpBadRequestException(string Message) {
      return new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) {
        Content = new ObjectContent<WsRespuesta<string>>(
          HerramientaRespuestas<string>.CrearRespuesta(
            Message, CatalogoRespuestas.SOLICITUD_INCORRECTA.Codigo, CatalogoRespuestas.SOLICITUD_INCORRECTA.Mensaje, false),
          new JsonMediaTypeFormatter(), "application/json")
      });
    }
  }
}