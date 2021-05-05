using cmv.tecnologia.DAL;
using cmv.tecnologia.DAL.WsCalixta;
using cmv.tecnologia.Entidades.Catalogos;
using cmv.tecnologia.Entidades.Excepciones;
using cmv.tecnologia.Entidades.Herramientas;
using cmv.tecnologia.Entidades.Notificacion;
using cmv.tecnologia.NotificationService.Tools;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace cmv.tecnologia.NotificationService.Controllers {
    [Authorize]
    [RoutePrefix("notification")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NotificationController : ApiController {
        private NotificationDAO DAO;
        public NotificationController(NotificationDAO DAO) {
          this.DAO = DAO;
        }

        /// <summary>
        /// Envia mensaje de texto de CMV
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("enviar_sms")]
        public IHttpActionResult SendSmsNotificationTextMessage([FromBody] Notificacion Request) {
            if (!RequestValidator.SmsNotificationValidator(Request, out string eMessages))
            throw HttpResponseTool<string>.BuildHttpBadRequestException(eMessages);
            try {
            if (DAO.EnviarSms(Request))
                return Json(HerramientaRespuestas<bool>.CrearRespuestaExitosa(true));
            else
                return Json(HerramientaRespuestas<string>.CrearRespuesta(null, CatalogoRespuestas.ERROR_NOTIFICACION.Codigo, CatalogoRespuestas.ERROR_NOTIFICACION.Mensaje, false));
            } catch (NotificationException e) {
            throw HttpResponseTool<NotificationException>.BuildHttpErrorException(e);
            } catch (Exception e) {
            throw HttpResponseTool<string>.BuildHttpErrorException(e);
            }
        }

        /// <summary>
        /// Envia correo electronico externo
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("enviar_mail")]
        public IHttpActionResult SendEMailNotificationMessageCmv([FromBody] Notificacion Request) {
          if (!RequestValidator.EmailNotificationValidator(Request, true, out string eMessages))
            throw HttpResponseTool<string>.BuildHttpBadRequestException(eMessages);
          try {
            if (DAO.EnviarMail(Request))
              return Json(HerramientaRespuestas<bool>.CrearRespuestaExitosa(true));
            else
              return Json(HerramientaRespuestas<string>.CrearRespuesta(null, CatalogoRespuestas.ERROR_NOTIFICACION.Codigo, CatalogoRespuestas.ERROR_NOTIFICACION.Mensaje, false));
          } catch (NotificationException e) {
            throw HttpResponseTool<NotificationException>.BuildHttpErrorException(e);
          } catch (Exception e) {
            throw HttpResponseTool<string>.BuildHttpErrorException(e);
          }
        }

        /// <summary>
        /// Consulta el saldo actual disponible para envio de notificaciones
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("consultar_saldo_notificaciones")]
        public IHttpActionResult CheckBalance()
        {
            /*
            if (!RequestValidator.SmsNotificationValidator(Request, out string eMessages))
                throw HttpResponseTool<string>.BuildHttpBadRequestException(eMessages);
            */
            try
            {
                Saldos responseConsultaSaldo = new Saldos();
                responseConsultaSaldo = DAO.ConsultarSaldoDisponible();

                if (responseConsultaSaldo.id == 0)
                    return Json(HerramientaRespuestas<bool>.CrearRespuestaExitosa(true,"Saldo Disponible: "+ responseConsultaSaldo.disponible));
                else
                    return Json(HerramientaRespuestas<string>.CrearRespuesta(null, CatalogoRespuestas.ERROR_NOTIFICACION.Codigo, CatalogoRespuestas.ERROR_NOTIFICACION.Mensaje, false));
            }
            catch (NotificationException e)
            {
                throw HttpResponseTool<NotificationException>.BuildHttpErrorException(e);
            }
            catch (Exception e)
            {
                throw HttpResponseTool<string>.BuildHttpErrorException(e);
            }
        }

        /// <summary>
        /// Consulta el estado de el envio de un mail con respecto a un id de mail
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("estado_mail_enviado")]
        public IHttpActionResult StatusMail([FromBody] Notificacion Request)
        {
            /*
            if (!RequestValidator.SmsNotificationValidator(Request, out string eMessages))
                throw HttpResponseTool<string>.BuildHttpBadRequestException(eMessages);
            */
            try
            {
                if (DAO.ConsultaEstadoMailEnviado(Request))
                    return Json(HerramientaRespuestas<bool>.CrearRespuestaExitosa(true));
                else
                    return Json(HerramientaRespuestas<string>.CrearRespuesta(null, CatalogoRespuestas.ERROR_NOTIFICACION.Codigo, CatalogoRespuestas.ERROR_NOTIFICACION.Mensaje, false));
            }
            catch (NotificationException e)
            {
                throw HttpResponseTool<NotificationException>.BuildHttpErrorException(e);
            }
            catch (Exception e)
            {
                throw HttpResponseTool<string>.BuildHttpErrorException(e);
            }

        }

        /// <summary>
        /// Envia notificaciones segun correponda su tipo de manera asincrona
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("cmv_finanzas_send_notifications_async")]
        public IHttpActionResult SendNotificationCmv([FromBody] List<Notificacion> request)
        {
            if (!RequestValidator.NotificationValidator(request, true, out string eMessages))
                throw HttpResponseTool<string>.BuildHttpBadRequestException(eMessages);
            try
            {
                if (DAO.EnviarNotificacionesAsync(request))
                    return Json(HerramientaRespuestas<bool>.CrearRespuestaExitosa(true));
                else
                    return Json(HerramientaRespuestas<string>.CrearRespuesta(null, CatalogoRespuestas.ERROR_NOTIFICACION.Codigo, CatalogoRespuestas.ERROR_NOTIFICACION.Mensaje, false));
            }
            catch (NotificationException e)
            {
                throw HttpResponseTool<NotificationException>.BuildHttpErrorException(e);
            }
            catch (Exception e)
            {
                throw HttpResponseTool<string>.BuildHttpErrorException(e);
            }
        }

        /// <summary>
        /// Envia mensaje de correo desde Robot Cmv
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("send_local_mail")]
        public IHttpActionResult SendLocalEMailNotificationMessage([FromBody] EmailNotification Request) {
          if (!RequestValidator.EmailNotificationValidator(Request, false, out string eMessages))
            throw HttpResponseTool<string>.BuildHttpBadRequestException(eMessages);
          try {
            if (DAO.SendLocalMail(Request))
              return Json(HerramientaRespuestas<bool>.CrearRespuestaExitosa(true));
            else
              return Json(HerramientaRespuestas<string>.CrearRespuesta(null, CatalogoRespuestas.ERROR_NOTIFICACION.Codigo, CatalogoRespuestas.ERROR_NOTIFICACION.Mensaje, false));
          } catch (NotificationException e) {
            throw HttpResponseTool<NotificationException>.BuildHttpErrorException(e);
          } catch (Exception e) {
            throw HttpResponseTool<string>.BuildHttpErrorException(e);
          }
        }

        /// <summary>
        /// Envia mensaje de correo
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("send_mail")]
        public IHttpActionResult SendEMailNotificationMessage([FromBody] EmailNotification Request) {
          if (!RequestValidator.EmailNotificationValidator(Request, true, out string eMessages))
            throw HttpResponseTool<string>.BuildHttpBadRequestException(eMessages);
          try {
            if (DAO.SendMail(Request))
              return Json(HerramientaRespuestas<bool>.CrearRespuestaExitosa(true));
            else
              return Json(HerramientaRespuestas<string>.CrearRespuesta(null, CatalogoRespuestas.ERROR_NOTIFICACION.Codigo, CatalogoRespuestas.ERROR_NOTIFICACION.Mensaje, false));
          } catch (NotificationException e) {
            throw HttpResponseTool<NotificationException>.BuildHttpErrorException(e);
          } catch (Exception e) {
            throw HttpResponseTool<string>.BuildHttpErrorException(e);
          }
        }
    }
}
