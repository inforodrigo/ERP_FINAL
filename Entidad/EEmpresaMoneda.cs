using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class EEmpresaMoneda
    {
        public int Id { get; set; }
        public double Cambio { get; set; }
        public int Activo { get; set; }
        
        public DateTime FechaRegistro { get; set; }
        public int idEmpresa { get; set; }
        public int idMonedaPrincipal { get; set; }
        public int idMonedaAlternativa { get; set; }
        public int idUsuario { get; set; }

        public string Fecha { get; set; }

        [DisplayName("Moneda Principal")]
        public string MonedaPrincipal { get; set; }

        [DisplayName("Moneda Alternativa")]
        public string MonedaAlternativa { get; set; }
    }
}
