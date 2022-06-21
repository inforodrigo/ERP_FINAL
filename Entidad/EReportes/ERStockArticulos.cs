using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad.EReportes
{
    public class ERStockArticulos
    {
        public int IdArticulo { get; set; }
        public int IdCategoria { get; set; }
        public string Articulo { get; set; }
        public string Categoria { get; set; }
        public int Stock { get; set; }
    }
}
