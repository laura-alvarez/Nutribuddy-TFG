using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NutriBuddy.Models
{
    public class CentroForm
    {
        public int IDCentro { get; set; }
        public string Nombre { get; set; }
        public string CIF { get; set; }
        public string Descripcion { get; set; }
        public int IDPlan { get; set; }
        public int MaxTrabajadores { get; set; }
        public int TrabajadoresActuales { get; set; }
        public int IDProvincia { get; set; }
        public string Direccion { get; set; }
        public string Localidad { get; set; }
        public string CP { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public bool Consultation_F2F { get; set; }
        public bool Consultation_Online { get; set; }
        public string NombreR { get; set; }
        public string ApellidosR { get; set; }
        public string DNIR { get; set; }
        public string SSR { get; set; }
        public string NacionalidadR { get; set; }
        public int IDProvinciaR { get; set; }
        public string LocalidadR { get; set; }
        public string EmailR { get; set; }
        public string TelefonoR { get; set; }
    }

    public class CentroPropio
    {
        public string Nombre { get; set; }
        public string Provincia { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }

        public bool EsPresencial { get; set; }
        public bool EsOnline { get; set; }
        public List<int> IdPuestos { get; set; }
        public int IdProvincia { get; set; }
    }

    public class CentroListado
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Plan { get; set; }
        public string Provincia { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Responsable { get; set; }
    }

    public class CentroBasico
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
    }


}