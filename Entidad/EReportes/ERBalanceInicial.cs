using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad.EReportes
{
    public class ERBalanceInicialActivo
    {
        public string CuentaCabezera { get; set; }
        public string Cuenta { get; set; }
        public double Monto { get; set; }
        public double MontoTotal { get; set; } 
    }

    public class ERBalanceInicialPasivoPatrimonio
    {
        public string CuentaCabezera { get; set; }     
        public string Cuenta { get; set; }
        public double Monto { get; set; }
        public double MontoTotal { get; set; }
    }
}
