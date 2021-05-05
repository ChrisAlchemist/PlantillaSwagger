using cmv.tecnologia.DAO.Herramientas;
using cmv.tecnologia.Entidades.Catalogos;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmv.tecnologia.DAO
{
    public class CatalogoDAO
    {
        public dynamic  ObtenerNombres(out dynamic Estatus)
        {
            //var parametros = new DynamicParameters();
            //parametros.Add("");
            var result = ConstructorDapper.ConsultaDapperLista(CatalogoSP.CATALOGO._OBTIENE_PAISES, null, out Estatus);
            return result;
        }
    }
}
