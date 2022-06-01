using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class EArticulo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public double PrecioVenta { get; set; }
        public int IdEmpresa { get; set; }
        public int IdUsuario { get; set; }
        public int DemandaDiaria { get; set; }
        public string TiempoEsperaDiario { get; set; }
        public string TiempoEsperaMaxDiario { get; set; }
        public double CostoOrden { get; set; }
        public double CostoInventario { get; set; }
        public string PuntoNuevoPedido { get; set; }
    }
}
