using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class ENota
    {
        public int Id { get; set; }
        public int NroNota { get; set; }
        public DateTime Fecha { get; set; }
        public string FechaStr { get; set; }
        public string Descripcion { get; set; }
        public double Total { get; set; }
        public int Tipo { get; set; }
        public int IdEmpresa { get; set; }
        public int IdUsuario { get; set; }
        public int IdComprobante { get; set; }
        public int Estado { get; set; }
    }
}
