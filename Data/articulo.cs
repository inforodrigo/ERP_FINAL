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
    
    public partial class articulo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public articulo()
        {
            this.categoria = new HashSet<categoria>();
        }
    
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public Nullable<int> cantidad { get; set; }
        public double precioVenta { get; set; }
        public int idEmpresa { get; set; }
        public int idUsuario { get; set; }
        public Nullable<int> demandaDiaria { get; set; }
        public string tiempoEsperaDiario { get; set; }
        public string tiempoEsperaMaxDiario { get; set; }
        public Nullable<double> costoOrden { get; set; }
        public Nullable<double> costoInventario { get; set; }
        public string puntoNuevoPedido { get; set; }
    
        public virtual empresa empresa { get; set; }
        public virtual usuario usuario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<categoria> categoria { get; set; }
    }
}