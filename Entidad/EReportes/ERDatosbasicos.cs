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
        public string NombreEmpresa { get; set; }
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
    public class ERDatosBasicoSumasSaldos
    {
        public string Usuario { get; set; }
        public string NombreEmpresa { get; set; }
        public string Gestion { get; set; }
        public string Moneda { get; set; }
        public string FechaActual { get; set; }
    }
    public class ERDatosBasicoBalanceInicial
    {
        public string Usuario { get; set; }
        public string NombreEmpresa { get; set; }
        public string Gestion { get; set; }
        public string Moneda { get; set; }
        public string FechaActual { get; set; }
    }
    public class ERDatosBasicoLibroDiario
    {
        public string Usuario { get; set; }
        public string NombreEmpresa { get; set; }
        public string Gestion { get; set; }
        public string Periodo { get; set; }
        public string Moneda { get; set; }
        public string FechaActual { get; set; }
    }
    public class ERDatosBasicoLibroMayor
    {
        public string Usuario { get; set; }
        public string NombreEmpresa { get; set; }
        public string Gestion { get; set; }
        public string Periodo { get; set; }
        public string Moneda { get; set; }
        public string FechaActual { get; set; }
    }

    public class ERDatosBasicoNotaCompra
    {
        public string Usuario { get; set; }
        public string NombreEmpresa { get; set; }       
        public string FechaActual { get; set; }
    }

    public class ERDatosBasicoNotaVenta
    {
        public string Usuario { get; set; }
        public string NombreEmpresa { get; set; }
        public string FechaActual { get; set; }
    }

    public class ERDatosBasicoEstadoResultado
    {
        public string Usuario { get; set; }
        public string NombreEmpresa { get; set; }
        public string Gestion { get; set; }
        public string Moneda { get; set; }
        public string FechaActual { get; set; }
    }

    public class ERDatosBasicoBalanceGeneral
    {
        public string Usuario { get; set; }
        public string NombreEmpresa { get; set; }
        public string Gestion { get; set; }
        public string Moneda { get; set; }
        public string FechaActual { get; set; }
    }

}
