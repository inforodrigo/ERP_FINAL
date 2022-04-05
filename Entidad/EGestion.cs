using Data;
using Entidad.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class EGestion : EValidaciones
    {
        public int Id { get; set; }

        [DisplayName("Nombre")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [RegularExpression(ExpLtrNro, ErrorMessage = MsgLtrNro)]
        public string Nombre { get; set; }

        [DisplayName("Fecha de inicio")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public DateTime? FechaInicio { get; set; }

        [DisplayName("Fecha fin")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public DateTime? FechaFin { get; set; }

        public string fechaIni { get; set; }
        public string fechaFi { get; set; }

        public int IdUsuario { get; set; }
        public int IdEmpresa { get; set; }
        public string Usuario { get; set; }
        public string Empresa { get; set; }
        public int Estado { get; set; }
    }
}
