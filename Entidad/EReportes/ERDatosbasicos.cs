using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad.EReportes
{
    public class ERDatosbasicos
    {
        public string Usuario { get; set; }
        public string FechaActual { get; set; }

    }

    public class ERDatosBasicoGestion
    {
        public string Usuario { get; set; }
        public string NombreEmpresa { get; set; }
        public string FechaActual { get; set; }

    }

    public class ERDatosBasicoComprobante
    {
        public string Usuario { get; set; }
        public string NombreEmpresa { get; set; }
        public string FechaActual { get; set; }
    }
}
