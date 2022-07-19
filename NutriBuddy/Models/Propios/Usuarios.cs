using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NutriBuddy.Models.Propios
{
    public class Usuarios
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int IDRol { get; set; }
        public int IDProvincia { get; set; }
    }

    public class TrabajadorCForm
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public List<int> Puestos { get; set; }
        public string SS { get; set; }
        public string DNI { get; set; }
        public string Nacionalidad { get; set; }
        public int IDProvincia { get; set; }
        public string Localidad { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
    }
    public class FormCambioContrasenya
    {
        public string Email { get; set; }
        public string CodigoRecuperacion { get; set; }
        public string NuevaContrasenya { get; set; }
    }

    public class FormRegistrar
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Contrasenya { get; set; }
        public string Dni { get; set; }
        public string Direccion { get; set; }
        public int Provincia { get; set; }
        public string Telefono { get; set; }
        public string FechaNacimiento { get; set; }
        public bool EsHombre { get; set; }
        public int Centro { get; set; }
    }
}