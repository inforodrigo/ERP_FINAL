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
    
    public partial class gestion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public gestion()
        {
            this.periodo = new HashSet<periodo>();
        }
    
        public int id { get; set; }
        public string nombre { get; set; }
        public System.DateTime fechainicio { get; set; }
        public System.DateTime fechafin { get; set; }
        public int idUsuario { get; set; }
        public int idEmpresa { get; set; }
        public int estado { get; set; }
    
        public virtual empresa empresa { get; set; }
        public virtual usuario usuario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<periodo> periodo { get; set; }
    }
}