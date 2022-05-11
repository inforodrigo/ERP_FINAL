using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad.EReportes
{
    public class ERLibroMayor
    {
        public string Cuenta { get; set; }
        public string Fecha { get; set; }
        public int NroComprobante { get; set; }
        public string Tipo { get; set; }
        public string Glosa { get; set; }
        public double Debe { get; set; }
        public double Haber { get; set; }
        public double Saldo { get; set; }
    }
}
