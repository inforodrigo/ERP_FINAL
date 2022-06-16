using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad.EReportes
{
    public class ERBalanceGeneral
    {

    }

    public class ECabeceraGeneralActivo
    {
        public string CodigoActivo { get; set; }
        public string CuentaActivo { get; set; }
        public double TotalActivo { get; set; }
    }

    public class ECabeceraGeneralPasivoPatrimonio
    {
        public string CodigoPasivo { get; set; }
        public string CuentaPasivo { get; set; }
        public string CodigoPatrimonio { get; set; }
        public string CuentaPatrimonio { get; set; }
        public double TotalPasivoPatrimonio { get; set; }
    }

    public class EBalanceGeneralActivo
    {
        public string CodigoCuenta { get; set; }
        public string Cuenta { get; set; }
        public double Debe { get; set; }
        public double TotalGeneral { get; set; }
        public int IdCabecera1 { get; set; }
        public string CodigoCabecera1 { get; set; }
        public string CuentaCabecera1 { get; set; }
        public double TotalCabecera1 { get; set; }
        public int IdCabecera2 { get; set; }
        public string CodigoCabecera2 { get; set; }
        public string CuentaCabecera2 { get; set; }
        public double TotalCabecera2 { get; set; }
        public int IdCabecera3 { get; set; }
        public string CodigoCabecera3 { get; set; }
        public string CuentaCabecera3 { get; set; }
        public double TotalCabecera3 { get; set; }
        public int IdCabecera4 { get; set; }
        public string CodigoCabecera4 { get; set; }
        public string CuentaCabecera4 { get; set; }
        public double TotalCabecera4 { get; set; }
        public int IdCabecera5 { get; set; }
        public string CodigoCabecera5 { get; set; }
        public string CuentaCabecera5 { get; set; }
        public double TotalCabecera5 { get; set; }
    }

    public class EBalanceGeneralPasivoPatrimonio
    {
        public string CodigoCuenta { get; set; }
        public string Cuenta { get; set; }
        public double Haber { get; set; }
        public double TotalGeneral { get; set; }
        public int IdCabecera1 { get; set; }
        public string CodigoCabecera1 { get; set; }
        public string CuentaCabecera1 { get; set; }
        public double TotalCabecera1 { get; set; }
        public int IdCabecera2 { get; set; }
        public string CodigoCabecera2 { get; set; }
        public string CuentaCabecera2 { get; set; }
        public double TotalCabecera2 { get; set; }
        public int IdCabecera3 { get; set; }
        public string CodigoCabecera3 { get; set; }
        public string CuentaCabecera3 { get; set; }
        public double TotalCabecera3 { get; set; }
        public int IdCabecera4 { get; set; }
        public string CodigoCabecera4 { get; set; }
        public string CuentaCabecera4 { get; set; }
        public double TotalCabecera4 { get; set; }
        public int IdCabecera5 { get; set; }
        public string CodigoCabecera5 { get; set; }
        public string CuentaCabecera5 { get; set; }
        public double TotalCabecera5 { get; set; }
    }
}
