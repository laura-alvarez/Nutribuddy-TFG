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
    
    public partial class OpcionesDiarioTipo
    {
        public OpcionesDiarioTipo()
        {
            this.Diario = new HashSet<Diario>();
            this.Diario1 = new HashSet<Diario>();
        }
    
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string NombreEN { get; set; }
    
        public virtual ICollection<Diario> Diario { get; set; }
        public virtual ICollection<Diario> Diario1 { get; set; }
    }
}