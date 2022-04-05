using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class ELogin
    {
        [DisplayName("Usuario")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Nickname { get; set; }

        [DisplayName("Contraseña")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Pass { get; set; }

        public string LoginErrorMessage { get; set; }
    }
}
