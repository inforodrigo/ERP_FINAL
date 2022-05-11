using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad.EReportes
{
    public class ERSumasSaldos
    {
        public string Cuenta { get; set; }
        public double SumasDebe { get; set; }
        public double SumasHaber { get; set; }
        public double SaldosDebe { get; set; }
        public double SaldosHaber { get; set; }
        public double TotalSumasDebe { get; set; }
        public double TotalSumasHaber { get; set; }
        public double TotalSaldosDebe { get; set; }
        public double TotalSaldosHaber { get; set; }
    }
}
