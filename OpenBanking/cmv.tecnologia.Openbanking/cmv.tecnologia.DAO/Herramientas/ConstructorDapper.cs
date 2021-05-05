using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmv.tecnologia.DAO.Herramientas
{
    public static class ConstructorDapper
    {
        public static dynamic ConsultaDapperLista(string SP, DynamicParameters parameters, out dynamic Estatus)
        {
            using (var conexion = new SqlConnection(Conexion.ObtenerConexion()))
            {
                var result = conexion.QueryMultiple(SP, parameters, commandType: System.Data.CommandType.StoredProcedure);
                Estatus = result.Read().First();
                if(Estatus.codigo == 200)
                {
                    return result.Read();
                }
                return null;
            }
        }

        public static dynamic ConsultaDapper(string SP, DynamicParameters parameters)
        {
            using (var conexion = new SqlConnection(Conexion.ObtenerConexion()))
            {
                
                return conexion.QuerySingleOrDefault(SP, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public  static List<dynamic> ConsultaDapperListaDinamica(string SP, DynamicParameters parameters, out dynamic Estatus)
        {
            using (var conexion = new SqlConnection(Conexion.ObtenerConexion()))
            {
                List<dynamic> listResult = new List<dynamic>();
                var result = conexion.QueryMultiple(SP, parameters, commandType: CommandType.StoredProcedure);
                Estatus = result.Read().First();

                if (Estatus.codigo ==200)
                {
                    while (!result.IsConsumed)
                    {
                        listResult.Add(result.Read());
                    }                    
                }
                return listResult;
            }
        } 
    }
}
