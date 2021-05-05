using cmv.tecnologia.utilidades.EntidadesCalixta;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmv.tecnologia.utilidades
{
    public class DatosSesionCalixta
    {
  
        //private static string idCliente = string.Empty;
        //private static string email = string.Empty;
        //private static string encpwd = string.Empty;
        private static UsuarioCalixta usuarioCalixta = new UsuarioCalixta();

        public static UsuarioCalixta ObtenerDatosSesionCalixta()
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM");
                if (key != null)
                {
                    usuarioCalixta.idCliente = Convert.ToInt32(key.GetValue("idCliente").ToString());
                    usuarioCalixta.email = key.GetValue("email").ToString();
                    usuarioCalixta.encpwd = key.GetValue("encpwd").ToString();
                    usuarioCalixta.mailFrom = key.GetValue("mailFrom").ToString();

                    key.Close();
                }
                
                return usuarioCalixta; //@"Server=" + server + ";Database=" + bd + ";User Id=" + usuarioSa + ";Password=" + passUsuarioSa;
            }
            catch (Exception ex)
            {
                Logg.EscribirLog("Fecha:\n"+ DateTime.Now.ToString() +"\nExcepción: " + ex.Message+ "\n");
                throw ex;
            }
        }
    }
}
