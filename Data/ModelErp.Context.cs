﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbErpEntities : DbContext
    {
        public dbErpEntities()
            : base("name=dbErpEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<empresa> empresa { get; set; }
        public virtual DbSet<gestion> gestion { get; set; }
        public virtual DbSet<usuario> usuario { get; set; }
        public virtual DbSet<periodo> periodo { get; set; }
        public virtual DbSet<cuenta> cuenta { get; set; }
        public virtual DbSet<comprobante> comprobante { get; set; }
        public virtual DbSet<detalleComprobante> detalleComprobante { get; set; }
        public virtual DbSet<moneda> moneda { get; set; }
        public virtual DbSet<empresaMoneda> empresaMoneda { get; set; }
        public virtual DbSet<categoria> categoria { get; set; }
        public virtual DbSet<articulo> articulo { get; set; }
        public virtual DbSet<articuloCategoria> articuloCategoria { get; set; }
        public virtual DbSet<detalleNota> detalleNota { get; set; }
        public virtual DbSet<lote> lote { get; set; }
        public virtual DbSet<nota> nota { get; set; }      
    }
}
