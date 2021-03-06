using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class ELote
    {
        public int IdArticulo { get; set; }
        public string NombreArticulo { get; set; }
        public int NroLote { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string FechaIngresoStr { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string FechaVencimientoStr { get; set; }
        public int Cantidad { get; set; }
        public double PrecioCompra { get; set; }
        public int Stock { get; set; }
        public int IdNota { get; set; }
        public int Estado { get; set; }
        public string EstadoStr { get; set; }
    }
}
