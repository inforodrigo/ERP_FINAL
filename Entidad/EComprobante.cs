using System;
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
        public DateTime fecha { get; set; }
        public double tipoCambio { get; set; }
        public int Estado { get; set; }
        public int tipoComprobante { get; set; }
        public int idUsuario { get; set; }
        public int idMoneda { get; set; }
    }
}
