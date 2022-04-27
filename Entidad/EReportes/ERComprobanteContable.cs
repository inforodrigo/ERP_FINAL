using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad.EReportes
{
    public class ERComprobanteContable
    {
        public int Serie { get; set; }
        public string Estado { get; set; }
        public string Fecha { get; set; }
        public string Moneda { get; set; }
        public string TipoCambio { get; set; }
        public string TipoComprobante { get; set; }
        public string Glosa { get; set; }
        public string TotalDebe { get; set; }
        public string TotalHaber { get; set; }
    }
}
