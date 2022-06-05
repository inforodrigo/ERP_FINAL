using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class EConfiguracionIntegracion
    {
        public int IdEmpresa { get; set; }
        public int Integracion { get; set; }
        public int Caja { get; set; }
        public int CreditoFiscal { get; set; }
        public int DebitoFiscal { get; set; }
        public int Compras { get; set; }
        public int Ventas { get; set; }
        public int It { get; set; }
        public int ItxPagar { get; set; }
        public string CajaStr { get; set; }
        public string CreditoFiscalStr { get; set; }
        public string DebitoFiscalStr { get; set; }
        public string ComprasStr { get; set; }
        public string VentasStr { get; set; }
        public string ItStr { get; set; }
        public string ItxPagarStr { get; set; }
    }
}
