using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad.EReportes
{
    public class ERCabeceraActivo
    {
        public string CodigoActivo { get; set; }
        public string CuentaActivo { get; set; }
        public double TotalActivo { get; set; }        
    }

    public class ERCabeceraPasivoPatrimonio
    {
        public string CodigoPasivo { get; set; }     
        public string CuentaPasivo { get; set; }
        public string CodigoPatrimonio { get; set; }
        public string CuentaPatrimonio { get; set; }
        public double TotalPasivoPatrimonio { get; set; }
    }

    public class ERBalanceInicialActivo
    {
        public int IdCuentaCabezera1 { get; set; }
        public string CodigoCabecera1 { get; set; }
        public string CuentaCabecera1 { get; set; }
        public int IdCuentaCabezera2 { get; set; }
        public string CodigoCabecera2 { get; set; }
        public string CuentaCabecera2 { get; set; }
        public int IdCuentaCabezera3 { get; set; }
        public string CodigoCabecera3 { get; set; }
        public string CuentaCabecera3 { get; set; }
        public int IdCuentaCabezera4 { get; set; }
        public string CodigoCabecera4 { get; set; }
        public string CuentaCabecera4 { get; set; }
        public int IdCuentaCabezera5 { get; set; }
        public string CodigoCabecera5 { get; set; }
        public string CuentaCabecera5 { get; set; }
        public string CodigoCuenta { get; set; }
        public string Cuenta { get; set; }
        public double Debe { get; set; }
        public double TotalCabecera1 { get; set; }
        public double TotalCabecera2 { get; set; }
        public double TotalCabecera3 { get; set; }
        public double TotalCabecera4 { get; set; }
        public double TotalCabecera5 { get; set; }
        public double TotalGeneral { get; set; }
    }

    public class ERBalanceInicialPasivoPatrimonio
    {
        public int IdCuentaCabezera1 { get; set; }
        public string CodigoCabecera1 { get; set; }
        public string CuentaCabecera1 { get; set; }
        public int IdCuentaCabezera2 { get; set; }
        public string CodigoCabecera2 { get; set; }
        public string CuentaCabecera2 { get; set; }
        public int IdCuentaCabezera3 { get; set; }
        public string CodigoCabecera3 { get; set; }
        public string CuentaCabecera3 { get; set; }
        public int IdCuentaCabezera4 { get; set; }
        public string CodigoCabecera4 { get; set; }
        public string CuentaCabecera4 { get; set; }
        public int IdCuentaCabezera5 { get; set; }
        public string CodigoCabecera5 { get; set; }
        public string CuentaCabecera5 { get; set; }
        public string CodigoCuenta { get; set; }
        public string Cuenta { get; set; }
        public double Haber { get; set; }
        public double TotalCabecera1 { get; set; }
        public double TotalCabecera2 { get; set; }
        public double TotalCabecera3 { get; set; }
        public double TotalCabecera4 { get; set; }
        public double TotalCabecera5 { get; set; }
        public double TotalGeneral { get; set; }
    }
}
