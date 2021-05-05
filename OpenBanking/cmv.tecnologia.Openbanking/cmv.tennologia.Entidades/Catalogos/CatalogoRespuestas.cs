using cmv.tenologia.Entidades.Catalogos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmv.tecnologia.Entidades.Catalogos
{
    public class CatalogoRespuestas :TipoRespuesta
    {
        public static readonly CatalogoRespuestas SOLICITUD_CORRECTA = new CatalogoRespuestas(200, "SOLICITUD_CORRECTA");
        public static readonly CatalogoRespuestas ERROR_INTERNO = new CatalogoRespuestas(500, "ERROR_INTERNO");
        public static readonly CatalogoRespuestas SOLICITUD_INCORRECTA = new CatalogoRespuestas(500, "SOLICITUD_INCORRECTA");
        public static readonly CatalogoRespuestas NO_AUTORIZADO = new CatalogoRespuestas(401, "NO_AUROTIZADO");
        public CatalogoRespuestas(int Codigo, string Mensaje) : base (Codigo, Mensaje)
        {
            
        }
        
    }
}
