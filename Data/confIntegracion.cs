//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class confIntegracion
    {
        public int idEmpresa { get; set; }
        public int integracion { get; set; }
        public int caja { get; set; }
        public int creditoFiscal { get; set; }
        public int debitoFiscal { get; set; }
        public int compras { get; set; }
        public int ventas { get; set; }
        public int it { get; set; }
        public int itxPagar { get; set; }
    
        public virtual cuenta cuenta { get; set; }
        public virtual cuenta cuenta1 { get; set; }
        public virtual cuenta cuenta2 { get; set; }
        public virtual empresa empresa { get; set; }
        public virtual cuenta cuenta3 { get; set; }
        public virtual cuenta cuenta4 { get; set; }
        public virtual cuenta cuenta5 { get; set; }
        public virtual cuenta cuenta6 { get; set; }
    }
}
