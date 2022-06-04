using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad.EReportes
{
    public class ERDetalleNotaCompra
    {
        public string Articulo { get; set; }
        public int Cantidad { get; set; }
        public string Precio { get; set; }
        public string SubTotal { get; set; }       
    }
}
