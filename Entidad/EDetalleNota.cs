using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class EDetalleNota
    {
        public int IdArticulo { get; set; }
        public int NroLote { get; set; }
        public int IdNota { get; set; }
        public int Cantidad { get; set; }
        public double PrecioVenta { get; set; }
    }
}
