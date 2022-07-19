using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NutriBuddy.Models
{
    public class ContactoForm
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string QuienContacta { get; set; }
        public string Comentario { get; set; }

        public Dictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("##Nombre##", this.Nombre);
            result.Add("##Email##", this.Email);
            result.Add("##Telefono##", this.Telefono);
            result.Add("##QuienContacta##", this.QuienContacta);
            result.Add("##Comentario##", this.Comentario);
            return result;
        }
    }
}