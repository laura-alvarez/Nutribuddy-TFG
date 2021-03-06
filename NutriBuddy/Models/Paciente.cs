//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Paciente
    {
        public Paciente()
        {
            this.Diario = new HashSet<Diario>();
            this.PacienteMedidas = new HashSet<PacienteMedidas>();
            this.Cita = new HashSet<Cita>();
        }
    
        public int IDUsuario { get; set; }
        public string DNI { get; set; }
        public System.DateTime FechaNacimiento { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public bool EsHombre { get; set; }
        public Nullable<int> IDCentro { get; set; }
        public bool FormularioInicial { get; set; }
        public bool EsSedentario { get; set; }
        public Nullable<int> IDObjetivo { get; set; }
        public bool BajaAutoestima { get; set; }
        public bool TieneEstres { get; set; }
        public bool Activo { get; set; }
    
        public virtual Centro Centro { get; set; }
        public virtual ObjetivoTipo ObjetivoTipo { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<Diario> Diario { get; set; }
        public virtual ICollection<PacienteMedidas> PacienteMedidas { get; set; }
        public virtual ICollection<Cita> Cita { get; set; }
    }
}
