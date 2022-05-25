using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class ECategoria
    {
        public int Id { get; set; }
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int IdEmpresa { get; set; }
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string Empresa { get; set; }
        public string text { get; set; }
        public int IdCategoriaPadre { get; set; }
        public List<ECategoria> children { get; set; }
    }
}
