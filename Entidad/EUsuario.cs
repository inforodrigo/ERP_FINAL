using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class EUsuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Nickname { get; set; }
        public string Pass { get; set; }
        public int Tipo { get; set; }
        public int Estado { get; set; }
    }
}
