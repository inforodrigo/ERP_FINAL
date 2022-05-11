using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad.EReportes
{
    public class ERLibroDiario
    {
        public string Fecha { get; set; }
        public string Cuenta { get; set; }       
        public double Debe { get; set; }
        public double Haber { get; set; }
    }
}
