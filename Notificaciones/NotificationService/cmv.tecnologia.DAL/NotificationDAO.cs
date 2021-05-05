using cmv.tecnologia.DAL.WsCalixta;
using cmv.tecnologia.Entidades.Excepciones;
using cmv.tecnologia.Entidades.Notificacion;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Mime;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Web;
using cmv.tecnologia.utilidades;
using cmv.tecnologia.utilidades.EntidadesCalixta;

namespace cmv.tecnologia.DAL {
    public class NotificationDAO {
        WsCalixta.GatewayPortTypeClient wsCalixta = new WsCalixta.GatewayPortTypeClient();
        UsuarioCalixta usuarioCalixta = new UsuarioCalixta();
        
        public Boolean EnviarSms(Notificacion n) {
            bool smsEnviado = false;
            try
            {
                //SE INSTANCIAN LOS OBJETOS PROVENIENTES DEL WEB SERVICE     
                EnviaMensajeOLRefRequest requestEnviaSMS = new EnviaMensajeOLRefRequest();
                EnviaMensajeOLRefResponse responseEnviaSMS = new EnviaMensajeOLRefResponse();
                usuarioCalixta = DatosSesionCalixta.ObtenerDatosSesionCalixta();
                

                //SE SETEAN LOS ELEMENTOS QUE RESIVIRA EL WS
                requestEnviaSMS.idCliente = usuarioCalixta.idCliente; //id de cliente que proporciene Auronix
                requestEnviaSMS.email = usuarioCalixta.email; //email que se proporcinara para el envio de notificaciones
                requestEnviaSMS.password = usuarioCalixta.encpwd; //contraseña proporcionada con Auronix
                if (n.idTipoNotificacion == TIPO_NOTIFICACION.SMS || n.idTipoNotificacion == TIPO_NOTIFICACION.AMBOS)
                    requestEnviaSMS.tipo = "SMS";
                else
                {                    
                    throw new Exception("El tipo de notificación debe de ser SMS.");
                }

                if (n.celular != "" || n.celular != null)
                    requestEnviaSMS.telefono = n.celular;
                else
                {
                    throw new Exception("Se debe de ingresar un TELEFONO CELULAR para enviar un sms.");
                }
                requestEnviaSMS.mensaje = n.cuerpo;
                requestEnviaSMS.fechaInicio = DateTime.Now.ToString("dd/mm/yyyy/hh/mm");
                requestEnviaSMS.campoAux = ""; //se agregara algun id de referencia ?
                //requestEnviaSMS.idivr = 0; // es necesario?

                //SE CONSUME WS DE CALIXTA PARA ENVIAR EL SMS
                using (new OperationContextScope(wsCalixta.InnerChannel))
                {
                    responseEnviaSMS.resultado = wsCalixta.EnviaMensajeOL(requestEnviaSMS.idCliente, requestEnviaSMS.email, requestEnviaSMS.password, requestEnviaSMS.tipo, requestEnviaSMS.telefono, requestEnviaSMS.mensaje, requestEnviaSMS.idivr, requestEnviaSMS.fechaInicio, requestEnviaSMS.campoAux);                    
                }

                if (responseEnviaSMS.resultado == (int)Enumeraciones.CodigoEnviaMensajeOL.Enviado)
                {
                    Logg.EscribirLog("Fecha:\n" + DateTime.Now.ToString() + "\nMensaje: " + "SMS enviado conexito\n Celular: "+n.celular + "\nCodigo: " + responseEnviaSMS.resultado + "\n");
                    smsEnviado = true;
                }
                else
                {
                    Enumeraciones.CodigoEnviaMensajeOL codigoError = new Enumeraciones.CodigoEnviaMensajeOL();
                    
                    if (responseEnviaSMS.resultado >= 101 && responseEnviaSMS.resultado<=199)
                    {
                        throw new Exception("Falta de saldo en el servicio" + "\nCodigo: " + responseEnviaSMS.resultado);                        
                    }
                    else
                    {
                        codigoError = (Enumeraciones.CodigoEnviaMensajeOL)responseEnviaSMS.resultado;
                        throw new Exception(codigoError.ToString().Replace("_", " ") + "\nCodigo: " + responseEnviaSMS.resultado);
                    }                    
                }
            }
            catch (Exception ex)
            {
                Logg.EscribirLog("Fecha:\n" + DateTime.Now.ToString() + "\nMensaje: " + ex.Message + "\n");
                throw new Exception(ex.Message.Replace("\n",". "));
            }
            return smsEnviado;
        }

        public Boolean EnviarMail(Notificacion n) 
        {
            usuarioCalixta = DatosSesionCalixta.ObtenerDatosSesionCalixta();
            bool mailEnviado = false;
            string resultado = "";
            int tieneArchivoAdjunto = 0;
            byte[] archivoAdjunto = null;
            string nombreArchivo = "";
            byte[] CuerpoBase64EncodedBytes = System.Text.Encoding.UTF8.GetBytes(n.cuerpo);
            
            try
            {
                List<ArchivoAdjunto> lstAdjuntos = new List<ArchivoAdjunto>();
                foreach (var item in n.Adjunto)
                {
                    lstAdjuntos.Add(new ArchivoAdjunto() { Archivo = item.Archivo, Nombre = item.Nombre });
                    tieneArchivoAdjunto = 1;
                    archivoAdjunto = item.Archivo;
                    nombreArchivo = item.Nombre;
                }

                if (n.idTipoNotificacion != TIPO_NOTIFICACION.CORREO_ELECTRONICO && n.idTipoNotificacion != TIPO_NOTIFICACION.AMBOS)
                    throw new Exception("El tipo de notificación debe de ser CORREO ELECTRÓNICO.");
               
                if (n.correo == "" || n.correo == null)                
                    throw new Exception("Se debe de ingresar un CORREO ELECTRÓNICO.");

                //SE CONSUME WS DE CALIXTA PARA ENVIAR EL SMS
                using (new OperationContextScope(wsCalixta.InnerChannel))
                {
                    resultado = wsCalixta.EnviaEmail
                        (
                            usuarioCalixta.idCliente,                           //cte,  
                            usuarioCalixta.email,                               //email,
                            usuarioCalixta.encpwd,                              //password,
                            "Notificaciones CMV",                               //nombreCamp,
                            n.correo,                                           //to,
                            usuarioCalixta.mailFrom,                            //from,
                            "Caja Morelia Valladolid",                          //fromName,
                            usuarioCalixta.mailFrom,                            //replyTo,
                            n.asunto,                                           //subject,
                            0,                                                  //incrustarImagen,
                            n.asunto,                                           //textEmail
                            "<![CDATA["+n.cuerpo,                         //htmlEmail,
                            tieneArchivoAdjunto,                                //seleccionaAdjuntos,
                            tieneArchivoAdjunto == 1 ? archivoAdjunto : null,   //fileBase64,
                            tieneArchivoAdjunto == 1 ? nombreArchivo : null,    //fileNameBase64,
                            null,                                               //nombreArchivoPersonalizado,
                            tieneArchivoAdjunto == 1 ? 0 : 1,                   //envioSinArchivo,      
                            DateTime.Now.ToString("dd/mm/yyyy"),                //fechaInicio,   
                            Convert.ToInt32(DateTime.Now.ToString("hh")),       //horaInicio,
                            Convert.ToInt32(DateTime.Now.ToString("mm")),       //minutoInicio,
                            null,                                               //listasNegras,
                            null,                                               //referencia,
                            null                                                //campoAuxiliar
                        ); ;                                                
                }
                
                Enumeraciones.CodigoEnviaEmail codigoError = new Enumeraciones.CodigoEnviaEmail();
                bool success = Enum.IsDefined(typeof(Enumeraciones.CodigoEnviaEmail), Convert.ToInt32(resultado));
                if (success)
                {                    
                    codigoError = (Enumeraciones.CodigoEnviaEmail)Convert.ToInt32(resultado);
                    throw new Exception(codigoError.ToString().Replace("_", " ") + "\nCodigo: " + resultado);
                }
                else
                {
                    Logg.EscribirLog("Fecha:\n" + DateTime.Now.ToString() + "\nMensaje: " + "Estatus del correo: " + resultado + "\n Correo de destino: " + n.correo + "\nidMail: " + resultado + "\n");
                    mailEnviado = true;
                }
            }
            catch (Exception ex)
            {
                Logg.EscribirLog("Fecha:\n" + DateTime.Now.ToString() + "\nMensaje: " + ex.Message + "\n");
                throw new Exception(ex.Message.Replace("\n", ". "));
            }
            return mailEnviado;
        }

        public Saldos ConsultarSaldoDisponible()
        {
            Saldos responseConsultaSaldo = new Saldos();
            usuarioCalixta = DatosSesionCalixta.ObtenerDatosSesionCalixta();

            try
            {
                //SE CONSUME WS DE CALIXTA PARA ENVIAR EL SMS
                responseConsultaSaldo = wsCalixta.ConsultaSaldo(usuarioCalixta.idCliente, usuarioCalixta.email, usuarioCalixta.encpwd)[0];
                Logg.EscribirLog("Fecha:\n" + DateTime.Now.ToString() + "\nMensaje: " + "Consulta de saldo exitosa.- Saldo Disponible: "+responseConsultaSaldo.disponible + "\n");
            }
            catch (Exception ex)
            {
                Logg.EscribirLog("Fecha:\n" + DateTime.Now.ToString() + "\nMensaje: " + ex.Message + "\n");
                throw new Exception(ex.Message.Replace("\n", ". "));
            }
            
            return responseConsultaSaldo;
        } 

        public Boolean ConsultaEstadoMailEnviado(Notificacion n)
        {
            usuarioCalixta = DatosSesionCalixta.ObtenerDatosSesionCalixta();
            bool mailEnviado = false;
            int idEstatusMail ;
            try
            {
                //SE CONSUME WS DE CALIXTA PARA ENVIAR EL SMS

                idEstatusMail = wsCalixta.EstadoEnvioEmail(usuarioCalixta.idCliente, usuarioCalixta.email, usuarioCalixta.encpwd, n.idMail);

                if (idEstatusMail == (int)Enumeraciones.CodigoEstadoEnvioMail.Enviado)
                {
                    Logg.EscribirLog("Fecha:\n" + DateTime.Now.ToString() + "\nMensaje: " + "MAIL enviado con exito\n Celular: " + n.correo + "\nCodigo: " + idEstatusMail + "\n");
                    mailEnviado = true;
                }
                else
                {
                    Enumeraciones.CodigoEstadoEnvioMail codigoError = new Enumeraciones.CodigoEstadoEnvioMail();
                    codigoError = (Enumeraciones.CodigoEstadoEnvioMail)idEstatusMail;

                    if (idEstatusMail >= 101 && idEstatusMail <= 199)
                    {
                        throw new Exception("Falta de saldo en el servicio" + "\nCodigo: " + idEstatusMail);
                    }
                    else
                    {
                        codigoError = (Enumeraciones.CodigoEstadoEnvioMail)idEstatusMail;
                        throw new Exception(codigoError.ToString().Replace("_", " ") + "\nCodigo: " + idEstatusMail);
                    }
                    //Logg.EscribirLog("Fecha:\n" + DateTime.Now.ToString() + "\nMensaje: " + codigoError.ToString().Replace("_", " ")+"\nCodigo: "+ responseEnviaSMS.resultado+"\n");
                    //throw new Exception(codigoError.ToString().Replace("_", " ") + "\nCodigo: " + idEstatusMail);
                }
            }catch(Exception ex)
            {
                Logg.EscribirLog("Fecha:\n" + DateTime.Now.ToString() + "\nMensaje: " + ex.Message + "\n");
                throw new Exception(ex.Message.Replace("\n", ". "));
            }
            return mailEnviado;

        }
        
        public bool EnviarNotificacionesAsync(List<Notificacion> lstNotificaciones)
        {
        bool notificacionEnviada = false;
        try
        {
            
            if (lstNotificaciones != null)
            {
                if (lstNotificaciones.Count > 0)
                {
                    foreach (var item in lstNotificaciones)
                    {
                        switch ((TIPO_NOTIFICACION)item.idTipoNotificacion)
                        {
                            case TIPO_NOTIFICACION.SMS:
                                Thread HiloSms = new Thread(new ThreadStart(() => EnviarSms(item)));
                                HiloSms.Start();
                                    notificacionEnviada = true;
                            break;
                            case TIPO_NOTIFICACION.CORREO_ELECTRONICO:                                    
                                Thread HiloCorreo = new Thread(new ThreadStart(() => EnviarMail(item)));
                                HiloCorreo.Start();
                                    notificacionEnviada = true;
                            break;
                            case TIPO_NOTIFICACION.AMBOS:
                                    notificacionEnviada = true;
                                break;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
                throw new Exception(ex.Message);
        }
        return notificacionEnviada;
    }

        public Boolean SendLocalMail(EmailNotification Notification) {
      try {
        MailMessage email = new MailMessage();
        email.To.Add(new MailAddress(Notification.DestinationEmail));
        email.From = new MailAddress(ConfigurationManager.AppSettings["LMail"]);
        email.Subject = Notification.Subject;
        email.Priority = MailPriority.Normal;

        string FilePath = HttpContext.Current.Request.MapPath(@"~/Templates/Normal.html");
        StreamReader str = new StreamReader(FilePath);
        string MailText = str.ReadToEnd();
        str.Close();

        MailText = MailText.Replace("{MENSAJE}", Notification.Body);
        MailText = MailText.Replace("{DE}", Notification.From);

        LinkedResource logo;
        if (!string.IsNullOrEmpty(Notification.ImagenFirmaBase64)) {
          byte[] bytes = Convert.FromBase64String(Notification.ImagenFirmaBase64);
          Stream Stream = new MemoryStream(bytes);
          logo = new LinkedResource(Stream);
        } else {
          logo = new LinkedResource(HttpContext.Current.Request.MapPath(@"~/Resources/cmv_logo.jpg"));
        }
        logo.ContentId = "dpto";

        AlternateView av1 = AlternateView.CreateAlternateViewFromString(MailText, null, MediaTypeNames.Text.Html);
        av1.LinkedResources.Add(logo);

        email.AlternateViews.Add(av1);
        email.IsBodyHtml = true;

        Send(ConfigurationManager.AppSettings["LHost"],
          Convert.ToInt32(ConfigurationManager.AppSettings["LPort"]), new NetworkCredential(), email);
        email.Dispose();
        return true;
      } catch (Exception ex) {
        throw new NotificationException() {
          Mensaje = ex.Message
        };
      }
    }

        public Boolean SendMail(EmailNotification Notification) {
      try {
        MailMessage email = new MailMessage();
        email.To.Add(new MailAddress(Notification.DestinationEmail));
        email.From = new MailAddress(ConfigurationManager.AppSettings["Mail"]);
        email.Subject = Notification.Subject;
        email.Priority = MailPriority.Normal;

        string FilePath = HttpContext.Current.Request.MapPath(@"~/Templates/Normal.html");
        StreamReader str = new StreamReader(FilePath);
        string MailText = str.ReadToEnd();
        str.Close();

        MailText = MailText.Replace("{MENSAJE}", Notification.Body);
        MailText = MailText.Replace("{DE}", Notification.From);
        LinkedResource logo;
        if (!string.IsNullOrEmpty(Notification.ImagenFirmaBase64)) {
          byte[] bytes = Convert.FromBase64String(Notification.ImagenFirmaBase64);
          Stream Stream = new MemoryStream(bytes);
          logo = new LinkedResource(Stream);
        } else {
          logo = new LinkedResource(HttpContext.Current.Request.MapPath(@"~/Resources/cmv_logo.jpg"));
        }
        logo.ContentId = "dpto";

        AlternateView av1 = AlternateView.CreateAlternateViewFromString(MailText, null, MediaTypeNames.Text.Html);
        av1.LinkedResources.Add(logo);
        email.AlternateViews.Add(av1);
        email.IsBodyHtml = true;
        Send(ConfigurationManager.AppSettings["Host"],
          Convert.ToInt32(ConfigurationManager.AppSettings["Port"]), new NetworkCredential(ConfigurationManager.AppSettings["MailUser"], ConfigurationManager.AppSettings["MailPassword"]), email);
        email.Dispose();
        return true;
      } catch (Exception ex) {
        throw new NotificationException() {
          Mensaje = ex.Message
        };
      }
    }

        private Boolean Send(string Host, int Port, NetworkCredential Credentials, MailMessage mail) {
      try {
        SmtpClient smtp = new SmtpClient();
        smtp.Host = Host;
        smtp.Port = Port;
        smtp.EnableSsl = false;
        smtp.UseDefaultCredentials = false;
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtp.Credentials = Credentials;
        smtp.Send(mail);
        return true;
      } catch (Exception ex) {
        throw ex;
      }
    }
    }
}
