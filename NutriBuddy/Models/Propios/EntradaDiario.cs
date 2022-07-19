using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NutriBuddy.Models.Propios
{
    public class EntradaDiario
    {
        public string id { get; set; }
        public string title { get; set; }
        public bool allDay { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public string textColor { get; set; }
        public string display { get; set; }
    }

    public class Diario
    {
        public int ID { get; set; }
        public string Dia { get; set; }
        public List<Comida> Comidas { get; set; }
        public Medidas Medidas { get; set; }
        public int IDEstado { get; set; }
        public List<int> Emociones {get; set;}
        public decimal Agua { get; set; }
        public int IDSuenyo { get; set; }
        public string Medicamentos { get; set; }
        public bool Menstruaccion { get; set; }
        public bool Deporte { get; set; }
        public List<int> Deportes { get; set; }
        public string DeporteObservaciones { get; set; }

    }

    public class Comida
    {
        public int ID { get; set; }
        public int IDTipo { get; set; }
        public TimeSpan Hora { get; set; }
        public string HoraS { get; set; }
        public DateTime Dia { get; set; }
        public string Alimentos { get; set; }
        public string Observaciones { get; set; }
    }

    
}