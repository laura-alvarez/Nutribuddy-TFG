using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NutriBuddy.Controllers
{
    public class PerfilController : ParentController
    {
        // GET: Perfiloontroller
        public ActionResult Index()
        {
            return View();
        }
    }
}