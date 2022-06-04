using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad.EReportes
{
    public class ERNotaCompra
    {
        public int Nro { get; set; }
        public string Estado { get; set; }
        public string Fecha { get; set; }
        public string Proveedor { get; set; }
        public string Descripcion { get; set; }
        public string Total { get; set; }       
    }
}
