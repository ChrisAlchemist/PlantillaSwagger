using cmv.tecnologia.Entidades.Herramientas;
using cmv.tecnologia.Entidades.Notificacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace cmv.tecnologia.NotificationService.Tools {
  public class RequestValidator {
        /// <summary>
        /// Valida la notificacion de SMS
        /// </summary>
        /// <param name="Notification"></param>
        /// <param name="ErrorMessages"></param>
        /// <returns></returns>
        public static bool SmsNotificationValidator(SmsNotification Notification, out string ErrorMessages)
        {
            ErrorMessages = "";
            if (Notification == null)
            {
                ErrorMessages += "La notificacion no puede ser nula";
                return false;
            }
            if (string.IsNullOrEmpty(Notification.Message))
                ErrorMessages += "El mensaje no puede estar vacio o nulo, ";
            if (string.IsNullOrEmpty(Notification.Phone))
                ErrorMessages += "El telefono no puede estar vacio o nulo, ";
            if (!ValidationTool.PhoneValidator(Notification.Phone))
                ErrorMessages += "El telefono no es valido, intente con un formato de 10 digitos: ##########";
            return string.IsNullOrEmpty(ErrorMessages);
        }

        /// <summary>
        /// Valida el la notificacion de correo
        /// Si el type es false se trata de un mensaje al correo interno 
        /// </summary>
        /// <param name="Notification"></param>
        /// <param name="type"></param>
        /// <param name="ErrorMessages"></param>
        /// <returns></returns>
        public static bool EmailNotificationValidator(EmailNotification Notification, bool type, out string ErrorMessages)
        {
            ErrorMessages = "";
            if (Notification == null)
            {
                ErrorMessages += "La notificacion no puede ser nula";
                return false;
            }
            if (string.IsNullOrEmpty(Notification.From))
                ErrorMessages += "El remitente no puede estar vacio o nulo, ";
            if (string.IsNullOrEmpty(Notification.DestinationEmail))
                ErrorMessages += "El correo no puede estar vacio o nulo, ";
            if (string.IsNullOrEmpty(Notification.Body))
                ErrorMessages += "El cuerpo del mensaje no puede estar vacio o nulo, ";
            if (!ValidationTool.MailValidator(Notification.DestinationEmail))
                ErrorMessages += "El correo no es valido";
            if (!type && !Notification.DestinationEmail.Contains("@cmv.mx"))
                ErrorMessages += "El correo interno debe de tener @cmv.mx";
            return string.IsNullOrEmpty(ErrorMessages);
        }
        /// <summary>
        /// Valida la notificacion de SMS
        /// </summary>
        /// <param name="Notification"></param>
        /// <param name="ErrorMessages"></param>
        /// <returns></returns>
        public static bool SmsNotificationValidator(Notificacion Notification, out string ErrorMessages)
        {
            ErrorMessages = "";
            //if (Notification == null)
            //{
            //    ErrorMessages += "La notificacion no puede ser nula";
            //    return false;
            //}
            //if (string.IsNullOrEmpty(Notification.Message))
            //    ErrorMessages += "El mensaje no puede estar vacio o nulo, ";
            //if (string.IsNullOrEmpty(Notification.Phone))
            //    ErrorMessages += "El telefono no puede estar vacio o nulo, ";
            //if (!ValidationTool.PhoneValidator(Notification.Phone))
            //    ErrorMessages += "El telefono no es valido, intente con un formato de 10 digitos: ##########";
            return string.IsNullOrEmpty(ErrorMessages);
        }

        /// <summary>
        /// Valida el la notificacion de correo
        /// Si el type es false se trata de un mensaje al correo interno 
        /// </summary>
        /// <param name="Notification"></param>
        /// <param name="type"></param>
        /// <param name="ErrorMessages"></param>
        /// <returns></returns>
        public static bool EmailNotificationValidator(Notificacion Notification, bool type, out string ErrorMessages)
        {
            ErrorMessages = "";
            //if (Notification == null)
            //{
            //    ErrorMessages += "La notificacion no puede ser nula";
            //    return false;
            //}
            //if (string.IsNullOrEmpty(Notification.From))
            //    ErrorMessages += "El remitente no puede estar vacio o nulo, ";
            //if (string.IsNullOrEmpty(Notification.DestinationEmail))
            //    ErrorMessages += "El correo no puede estar vacio o nulo, ";
            //if (string.IsNullOrEmpty(Notification.Body))
            //    ErrorMessages += "El cuerpo del mensaje no puede estar vacio o nulo, ";
            //if (!ValidationTool.MailValidator(Notification.DestinationEmail))
            //    ErrorMessages += "El correo no es valido";
            //if (!type && !Notification.DestinationEmail.Contains("@cmv.mx"))
            //    ErrorMessages += "El correo interno debe de tener @cmv.mx";
            return string.IsNullOrEmpty(ErrorMessages);
        }

        /// <summary>
        /// Valida el la notificacion de correo
        /// Si el type es false se trata de un mensaje al correo interno 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="type"></param>
        /// <param name="ErrorMessages"></param>
        /// <returns></returns>
        public static bool NotificationValidator(List<Notificacion> request, bool type, out string ErrorMessages)
        {
            throw new NotImplementedException();
        }
    }
}