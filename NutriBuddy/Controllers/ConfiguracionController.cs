using NutriBuddy.Controllers.Utilidades;

using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Web.Mvc;
using System.Web.UI;

namespace NutriBuddy.Controllers
{
    public class ConfiguracionController : ParentController
    {
        public ActionResult CambiarIdioma(string idioma)
        {
            new ControladorIdiomas().ElegirIdioma(idioma);
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
       
    }
}