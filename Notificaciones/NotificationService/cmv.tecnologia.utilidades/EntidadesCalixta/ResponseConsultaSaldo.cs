using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmv.tecnologia.utilidades.EntidadesCalixta
{
    public class ResponseConsultaSaldo
    {
        public int id { get; set; }
        public string servicio { get; set; }
        public double disponible { get; set; }
    }
}
