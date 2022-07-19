using NutriBuddy.Controllers.Utilidades;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace NutriBuddy
{
    public class ParentController : Controller
    {
        public Dictionary<int, string> RUTAS = new Dictionary<int, string>()
        {
            {1,"~/Views/Perfil/PerfilCentro.cshtml"},
            {4,"~/Views/Perfil/Perfil.cshtml"},
            {5,"~/Views/Agenda/Agenda.cshtml"},
            {6,"~/Views/Centros/ListadoCentros.cshtml"},
            {7,"~/Views/Personal/Personal.cshtml"},
            {8,"~/Views/Pacientes/ListadoPacientes.cshtml"},
            {9,"~/Views/Diario/Diario.cshtml"},
            {1003,"~/Views/ZonaPrivada/_PZPaciente.cshtml"},
            {1002,"~/Views/Pacientes/MiCentro.cshtml"},
        };

        

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string idioma;
            HttpCookie ckIdioma = Request.Cookies["culture"];
            if (ckIdioma != null)
            {
                idioma = ckIdioma.Value;
            }
            else
            {
                var userLanguage = Request.UserLanguages;
                var userLang = userLanguage != null ? userLanguage[0] : "";
                if (userLang != "")
                {
                    idioma = userLang;
                }
                else
                {
                    idioma = ControladorIdiomas.IdiomaPorDefecto();
                }
            }
            Session["idioma"] = idioma;
            new ControladorIdiomas().ElegirIdioma(idioma);
            return base.BeginExecuteCore(callback, state);
        }
               
    }
}