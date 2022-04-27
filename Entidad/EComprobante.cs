using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class EComprobante
    {
        public int Id { get; set; }
        public int Serie { get; set; }
        public string Glosa { get; set; }
        public DateTime Fecha { get; set; }

        [DisplayName("Fecha")]
        public string Fechas { get; set; }

        [DisplayName("Tipo Cambio")]
        public double TipoCambio { get; set; }
        public int Estado { get; set; }

        [DisplayName("Tipo")]
        public int TipoComprobante { get; set; }
        public int IdEmpresa { get; set; }
        public int IdUsuario { get; set; }
        public int IdMoneda { get; set; }
        public string Moneda { get; set; }
        public string TipoComprobanteStr { get; set; }
        public string EstadoStr { get; set; }
    }
}
