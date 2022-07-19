using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NutriBuddy.Models.Propios
{
    public class PacienteInicial : Medidas
    {
        public int IdUsuario { get; set; }
        public int IDObjetivo { get; set; }
        public bool EsSedentario { get; set; }
        public bool TieneEstres { get; set; }
        public bool BajaAutoestima { get; set; }
    }

    public class Medidas
    {

        public decimal Altura { get; set; }
        public decimal Peso { get; set; }
        public decimal Pecho { get; set; }
        public decimal Cintura { get; set; }
        public decimal Cadera { get; set; }
        public decimal Grasa { get; set; }
        public decimal Musculo { get; set; }
        public decimal Agua { get; set; }
    }

    public class PacienteF
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string DNI { get; set; }
        public string Direccion { get; set; }
        public int IDProvincia { get; set; }
        public string Telefono { get; set; }
        public DateTime FNacimiento { get; set; }
        public bool EsHombre { get; set; }
    }

  
    public class PacienteT
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public DateTime ProxCita { get; set; }
        public string ProxCitaS { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
    }
}