namespace NutriBuddy.Models.Propios
{
    public class TrabajadorNB
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string SSR { get; set; }
        public int IDProvincia { get; set; }
        public string Provincia { get; set; }
        public string Email { get; set; }
    }

    public class TrabajadorC
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string DNI { get; set; }
        public string SSR { get; set; }
        public int IDProvincia { get; set; }
        public string Provincia { get; set; }
        public string Email { get; set; }
    }
}