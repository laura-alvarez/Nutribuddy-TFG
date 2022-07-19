using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace NutriBuddy.Controllers.Utilidades
{
    public class ControladorIdiomas
    {
        public static List<Idioma> IdiomasDisponibles = new List<Idioma> {
            new Idioma {
                Nombre = "Español", NombreCI = "es"
            },
            new Idioma {
                Nombre = "Inglés", NombreCI = "en"
            }                     
        };
        public static bool IdiomaDisponible(string idioma)
        {
            return IdiomasDisponibles.FirstOrDefault(a => a.NombreCI.Equals(idioma)) != null;
        }
        public static string IdiomaPorDefecto()
        {
            return IdiomasDisponibles[0].NombreCI;
        }
        public void ElegirIdioma(string idioma)
        {
            try
            {
                if (!IdiomaDisponible(idioma)) idioma = IdiomaPorDefecto();
                var cultureInfo = new CultureInfo(idioma);
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
                HttpCookie ckIdioma = new HttpCookie("culture", idioma)
                {
                    Expires = DateTime.Now.AddYears(1)
                };
                HttpContext.Current.Response.Cookies.Add(ckIdioma);
            }
            catch (Exception e) {
                Log.EscrbirLog("CAMBIO IDIOMA", e.Message, false);
            }
        }
    }

    public class Idioma
    {
        public string Nombre { get; set; }
        public string NombreCI { get; set; }

    }
}
