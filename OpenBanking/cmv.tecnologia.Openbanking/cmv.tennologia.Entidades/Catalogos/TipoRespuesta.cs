using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmv.tenologia.Entidades.Catalogos
{
    public class TipoRespuesta
    {
        public string Mensaje { get; set; }
        public int Codigo { get; set; }
        public TipoRespuesta(int Codigo, string Mensaje)
        {
            this.Codigo = Codigo;
            this.Mensaje = Mensaje;
        }
}
}
