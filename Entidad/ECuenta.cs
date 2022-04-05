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
    public class ECuenta : EValidaciones
    {
        public int Id { get; set; }
        public long IdCuenta { get; set; }

        [DisplayName("Codigo")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [RegularExpression(ExpLtrNro, ErrorMessage = MsgLtrNro)]

        public string Codigo { get; set; }

        [DisplayName("Nombre")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Nombre { get; set; }
        public string text { get; set; }
        public int Nivel { get; set; }
        public int TipoCuenta { get; set; }
        public int IdUsuario { get; set; }
        public int IdEmpresa { get; set; }
        public int IdCuentaPadre { get; set; }
        public string Usuario { get; set; }
        public string Empresa { get; set; }
        public int Estado { get; set; }
        public List<ECuenta> children { get; set; }
    }
}
