﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NutriBuddy.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class NutribuddyEntities : DbContext
    {
        public NutribuddyEntities()
            : base("name=NutribuddyEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Accion> Accion { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Rol_Accion> Rol_Accion { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<ComunidadAutonoma> ComunidadAutonoma { get; set; }
        public DbSet<Provincia> Provincia { get; set; }
        public DbSet<Plan> Plan { get; set; }
        public DbSet<Puesto> Puesto { get; set; }
        public DbSet<Contacto> Contacto { get; set; }
        public DbSet<TrabajadoresNB> TrabajadoresNB { get; set; }
        public DbSet<Trabajador> Trabajador { get; set; }
        public DbSet<CentroTrabajador> CentroTrabajador { get; set; }
        public DbSet<PuestoTrabajador> PuestoTrabajador { get; set; }
        public DbSet<Centro> Centro { get; set; }
        public DbSet<ComidaTipo> ComidaTipo { get; set; }
        public DbSet<DeporteTipo> DeporteTipo { get; set; }
        public DbSet<EstadoAnimicoDiario> EstadoAnimicoDiario { get; set; }
        public DbSet<EstadoAnimicoTipo> EstadoAnimicoTipo { get; set; }
        public DbSet<ObjetivoTipo> ObjetivoTipo { get; set; }
        public DbSet<OpcionesDiarioTipo> OpcionesDiarioTipo { get; set; }
        public DbSet<Paciente> Paciente { get; set; }
        public DbSet<Diario> Diario { get; set; }
        public DbSet<Deporte> Deporte { get; set; }
        public DbSet<Comida> Comida { get; set; }
        public DbSet<PacienteMedidas> PacienteMedidas { get; set; }
        public DbSet<Cita> Cita { get; set; }
    }
}