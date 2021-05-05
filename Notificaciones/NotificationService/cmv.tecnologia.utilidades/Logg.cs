using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmv.tecnologia.utilidades
{
    public class Logg
    {
        public static string EscribirLog(string mensaje)
        {
            string resultado = "todo bien";
            try
            {
                string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.ToString(), "LogNotificationService");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                System.IO.File.AppendAllText(Path.Combine(folder, @"Log_NotificactionService_" + DateTime.Now.Day.ToString("D2") + DateTime.Now.Month.ToString("D2") + DateTime.Now.Year + ".txt"), (mensaje + " \n"));
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
            }
            return resultado;
        }
    }
}
