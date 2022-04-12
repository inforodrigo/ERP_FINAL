using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class EEmpresaMoneda
    {
        public int Id { get; set; }
        public double Cambio { get; set; }
        public int activo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int idEmpresa { get; set; }
        public int idMonedaPrincipal { get; set; }
        public int idMonedaAlternativa { get; set; }
        public int idUsuario { get; set; }
    }
}
