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
    public class EEmpresa : EValidaciones
    {
        public int Id { get; set; }

        [DisplayName("Nombre")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [RegularExpression(ExpLtrNro, ErrorMessage = MsgLtrNro)]
        public string Nombre { get; set; }

        [DisplayName("Nit")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Nit { get; set; }

        [DisplayName("Sigla")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Sigla { get; set; }

        [DisplayName("Telefono")]
        public int Telefono { get; set; }

        [DisplayName("Correo")]
        [RegularExpression(ExpEmail, ErrorMessage = MsgEmail)]
        public string Correo { get; set; }

        [DisplayName("Direccion")]
        public string Direccion { get; set; }

        [DisplayName("Niveles")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int Niveles { get; set; }
        public int idUsuario { get; set; }

        [DisplayName("Usuario")]
        public string Usuario { get; set; }
        public int Estado { get; set; }

        public EEmpresa()
        {

        }
    }
}
