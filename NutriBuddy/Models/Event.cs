using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NutriBuddy.Models
{
    public class Event
    {
    }

    public class EventoCita 
    {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public string textColor { get; set; }
    }

    public class EventoCitaGuardar
    {
        public int ID { get; set; }
        public int IDTipo { get; set; }
        public int IDTrabajador { get; set; }
        public int IDPaciente { get; set; }
        public DateTime Dia { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string Titulo { get; set; }
        public string Observaciones { get; set; }
    }

    public class EventoCitaDetalles
    {
        public int ID { get; set; }
        public int IDTipo { get; set; }
        public int IDTrabajador { get; set; }
        public int IDPaciente { get; set; }
        public string Dia { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public string Titulo { get; set; }
        public string Observaciones { get; set; }
    }

}