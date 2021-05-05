using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmv.tecnologia.DAO
{
    public static class Conexion
    {
        private static string Server { get; set; }
        private static string BD { get; set; }
        private static string Usuario { get; set; }
        private static string Contrasena { get; set; }

        public static string ObtenerConexion()
        {
            if (ConfigurationManager.AppSettings["Enviroment"] == "prod")
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey("SYSTEM");
                if (key != null)
                {
                    BD = key.GetValue("BD").ToString();
                    Usuario = key.GetValue("Ususario").ToString();
                    Contrasena = key.GetValue("Contrasena").ToString();
                    Server = key.GetValue("Servidor").ToString();
                    key.Close();
                }
                else return string.Empty;
                return $"Server = {Server}; Database = {BD}; User Id = {Usuario};Password = {Contrasena}";
            }
            return $"{ConfigurationManager.ConnectionStrings[$"{ConfigurationManager.AppSettings["Enviroment"]}BD"].ConnectionString};User Id = sa_temp; Password = Abcde1";

        }

    }
}
