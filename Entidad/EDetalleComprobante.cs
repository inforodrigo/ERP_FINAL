using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class EDetalleComprobante
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public string Glosa { get; set; }
        public double montoDebe { get; set; }
        public double montoHaber { get; set; }
        public double montoDebeAlt { get; set; }
        public double montoHaberAlt { get; set; }
        public int idUsuario { get; set; }
        public int idComprobante { get; set; }
        public int idCuenta { get; set; }
        public string Cuenta { get; set; }
    }
}
