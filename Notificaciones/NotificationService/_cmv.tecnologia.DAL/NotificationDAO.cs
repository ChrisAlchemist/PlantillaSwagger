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
using System.Threading;
using System.Web;

namespace cmv.tecnologia.DAL {
  public class NotificationDAO {

    public Boolean SendSms(Notificacion n) {
        try
        {
            
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return true;
    }

    public Boolean SendMailCmvFinanzas(Notificacion n) {
        try
        {
            List<ArchivoAdjunto> lstAdjuntos = new List<ArchivoAdjunto>();
            foreach (var item in n.Adjunto)
            {
                lstAdjuntos.Add(new ArchivoAdjunto() { Archivo = item.Archivo, Nombre = item.Nombre });
            }

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return true;
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
                                Thread HiloSms = new Thread(new ThreadStart(() => SendSms(item)));
                                HiloSms.Start();
                                    notificacionEnviada = true;
                            break;
                            case TIPO_NOTIFICACION.CORREO_ELECTRONICO:                                    
                                Thread HiloCorreo = new Thread(new ThreadStart(() => SendMailCmvFinanzas(item)));
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
