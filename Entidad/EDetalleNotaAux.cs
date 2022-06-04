using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class EDetalleNotaAux
    {
        public int idArticulo { get; set; }
        public int nroLote { get; set; }
        public string articulo { get; set; }
        public string fecha { get; set; }
        public int cantidad { get; set; }
        public double precio { get; set; }
        public double subtotal { get; set; }
    }
}
