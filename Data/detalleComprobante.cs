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
    
    public partial class detalleComprobante
    {
        public int id { get; set; }
        public int numero { get; set; }
        public string glosa { get; set; }
        public double montoDebe { get; set; }
        public double montoHaber { get; set; }
        public double montoDebeAlt { get; set; }
        public double montoHaberAlt { get; set; }
        public int idUsuario { get; set; }
        public int idComprobante { get; set; }
        public int idCuenta { get; set; }
    
        public virtual comprobante comprobante { get; set; }
        public virtual cuenta cuenta { get; set; }
        public virtual usuario usuario { get; set; }
    }
}
