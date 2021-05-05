using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmv.tecnologia.Entidades
{
    public class WsRespuesta<T>
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; }
        public T Modelo { get; set; }

    }
}
