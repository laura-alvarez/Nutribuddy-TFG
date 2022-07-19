using NutriBuddy.Controllers.Utilidades;
using NutriBuddy.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NutriBuddy.Resources;
using NutriBuddy.Models.Propios;
using System.Web.Script.Serialization;

namespace NutriBuddy.Controllers
{
    public class HomeController : ParentController
    {
        public ActionResult Index()
        {
            Session["EsZP"] = false;
            ViewBag.Title = "NutriBuddy";
            return View();
        }

        public ActionResult Registro()
        {
            ViewBag.Title = "NutriBuddy - Registro";
            using (var db = new NutribuddyEntities())
            {
                Dictionary<string, List<ProvinciaBasica>> localizaciones = new Dictionary<string, List<ProvinciaBasica>>();
                var ccaa = (from ca in db.ComunidadAutonoma select ca).OrderBy(x => x.Nombre).ToList();
                foreach (ComunidadAutonoma ca in ccaa)
                {
                    var provincias = (from p in db.Provincia where p.IDCCAA == ca.ID select new ProvinciaBasica() { ID = p.ID, Nombre = p.Nombre }).OrderBy(x => x.Nombre).ToList();
                    localizaciones.Add(ca.Nombre, provincias);
                }
                ViewData["localizaciones"] = localizaciones;
            }
            return View();
        }

        public ActionResult GetIdioma()
        {
            string idioma = ControladorIdiomas.IdiomaPorDefecto();
            HttpCookie ckIdioma = Request.Cookies["culture"];
            if (ckIdioma != null)
            {
                idioma = ckIdioma.Value;
            }
            return Json(new { idioma = idioma }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Login()
        {
            ViewBag.Title = "NutriBuddy - Login";

            return View("/Views/Login/Login.cshtml");
        }


        public ActionResult DoLogin(User usuario)
        {

            if (string.IsNullOrEmpty(usuario.Email) || string.IsNullOrEmpty(usuario.Password)) return Json(new { flag = 3, msg = mensajesError.login_msg2 });
            using (var db = new NutribuddyEntities())
            {

                var usuarioBD = (from u in db.Usuario where u.Email == usuario.Email && u.Activo select u).FirstOrDefault();
                if (usuarioBD == null) return Json(new { flag = 2, msg = mensajesError.login_msg2 });
                string aux = Seguridad.DesencriptarAES(usuarioBD.Password, usuarioBD.FechaAlta.ToString(new CultureInfo("es-ES")));
                if (Seguridad.DesencriptarAES(usuarioBD.Password, usuarioBD.FechaAlta.ToString(new CultureInfo("es-ES"))).Equals(usuario.Password))
                {
                    Session["usuario"] = usuarioBD;
                    Session["usuarioID"] = usuarioBD.ID;
                    var permisos = (from p in db.Rol_Accion where p.IDRol == usuarioBD.IDRol && p.Activo select p.IDAccion).ToList();                    
                    var paciente = (from p in db.Paciente where p.IDUsuario == usuarioBD.ID select p).FirstOrDefault();
                    if (usuarioBD.IDRol == 5 && paciente.IDCentro == null) //SI NO TIENE CENTRO SE LE QUITA EL PERMISO DE VER LA PARTE DE MI CENTRO
                    {
                        permisos = permisos.Where(x => x != 1002).ToList();
                    }
                    if (permisos.Contains(1003))
                    {
                        permisos = permisos.OrderByDescending(x => x).ToList();
                    }
                    Session["permisos"] = permisos;
                    bool FI = usuarioBD.IDRol == 5 && paciente != null && !paciente.FormularioInicial;
                    return Json(new { flag = 0, FI = FI });
                }
                return Json(new { flag = 1, msg = mensajesError.login_msg1 });
            }
        }

        public ActionResult RecoverPass()
        {
            return View("/Views/Login/RecoverPass.cshtml");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AcercaNB()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        public ActionResult Contacto(ContactoForm datos)
        {
            if (string.IsNullOrEmpty(datos.Email) || !Validador.ValidarEmail(datos.Email)) { return Json(new { flag = 1 }); }
            if (string.IsNullOrEmpty(datos.Telefono) || !Validador.ValidarTelefono(datos.Telefono)) { return Json(new { flag = 2 }); }
            int error = 0;
            using (var db = new NutribuddyEntities())
            {
                db.Contacto.Add(
                  new Contacto()
                  {
                      Email = datos.Email,
                      EsCentro = datos.QuienContacta == "Centro",
                      Nombre = datos.Nombre,
                      Telefono = datos.Telefono,
                      Comentario = datos.Comentario.Length > 500 ? datos.Comentario.Substring(0, 499) : datos.Comentario,
                      FechaEnvio = DateTime.Now
                  }
                );

                try
                {
                    db.SaveChanges();
                    error = Email.EnviarEmail("Nuevo formulario de contacto", "Content\\Templates\\NuevoFormularioContacto-es.html", datos.ToDictionary());

                }
                catch (Exception e)
                {
                    Log.EscrbirLog("Formulario contacto", e.Message + " - " + e.StackTrace, false);
                }
            }
            return Json(new { flag = error });
        }

        public ActionResult Centros()
        {
            using (var db = new NutribuddyEntities())
            {
                Dictionary<string, List<ProvinciaBasica>> localizaciones = new Dictionary<string, List<ProvinciaBasica>>();
                var ccaa = (from ca in db.ComunidadAutonoma select ca).OrderBy(x => x.Nombre).ToList();
                foreach (ComunidadAutonoma ca in ccaa)
                {
                    var provincias = (from p in db.Provincia where p.IDCCAA == ca.ID select new ProvinciaBasica() { ID = p.ID, Nombre = p.Nombre }).OrderBy(x => x.Nombre).ToList();
                    localizaciones.Add(ca.Nombre, provincias);
                }
                ViewData["localizaciones"] = localizaciones;

                var puestos = (from p in db.Puesto
                               where p.ID > 1
                               select new Models.Propios.Puesto()
                               {
                                   IDPuesto = p.ID,
                                   Nombre = p.Nombre
                               }).ToList();
                ViewData["puestos"] = puestos;

                var centros = (from c in db.Centro
                               where c.Activo
                               select new CentroPropio()
                               {
                                   Nombre = c.NombreCentro,
                                   Provincia = c.Provincia.Nombre,
                                   Direccion = c.Direccion + ", " + c.Localidad + ", " + c.CP,
                                   Telefono = c.Telefono,
                                   EsPresencial = c.EsPresencial,
                                   EsOnline = c.EsOnline,
                                   IdProvincia = c.IDProvincia
                               }).ToList();

                ViewData["centros"] = centros;

                List<int> buscadores = new List<int> { -1, -1, -1 };
                ViewData["buscadores"] = buscadores;

                return View();
            }

        }

        public ActionResult Precios()
        {


            return View();
        }

        public ActionResult BuscarCentros(int bTipo, int bEsp, int bLoc)
        {
            using (var db = new NutribuddyEntities())
            {

                Dictionary<string, List<ProvinciaBasica>> localizaciones = new Dictionary<string, List<ProvinciaBasica>>();
                var ccaa = (from ca in db.ComunidadAutonoma select ca).OrderBy(x => x.Nombre).ToList();
                foreach (ComunidadAutonoma ca in ccaa)
                {
                    var provincias = (from p in db.Provincia where p.IDCCAA == ca.ID select new ProvinciaBasica() { ID = p.ID, Nombre = p.Nombre }).OrderBy(x => x.Nombre).ToList();
                    localizaciones.Add(ca.Nombre, provincias);
                }
                ViewData["localizaciones"] = localizaciones;


                var puestos = (from p in db.Puesto
                               where p.ID > 1
                               select new Models.Propios.Puesto()
                               {
                                   IDPuesto = p.ID,
                                   Nombre = p.Nombre
                               }).ToList();
                ViewData["puestos"] = puestos;

                var centros = (from c in db.Centro
                               where c.Activo
                               select new CentroPropio()
                               {
                                   Nombre = c.NombreCentro,
                                   Provincia = c.Provincia.Nombre,
                                   Direccion = c.Direccion + ", " + c.Localidad + ", " + c.CP,
                                   Telefono = c.Telefono,
                                   EsPresencial = c.EsPresencial,
                                   EsOnline = c.EsOnline,
                                   IdProvincia = c.IDProvincia,
                                   IdPuestos = (from p in db.PuestoTrabajador where p.IDCentro == c.ID select p.IDPuesto).Distinct().ToList()
                               }).ToList();
                if (bTipo != -1)
                {
                    switch (bTipo)
                    {
                        case 1: centros = centros.Where(x => x.EsPresencial).ToList(); break;
                        case 2: centros = centros.Where(x => x.EsOnline).ToList(); break;
                    }
                }
                if (bEsp != -1)
                {
                    centros = centros.Where(x => x.IdPuestos.Contains(bEsp)).ToList();
                }
                if (bLoc != -1)
                {
                    centros = centros.Where(x => x.IdProvincia == bLoc).ToList();
                }
                ViewData["centros"] = centros;

                List<int> buscadores = new List<int> { bTipo, bEsp, bLoc };
                ViewData["buscadores"] = buscadores;

                return View("~/Views/Home/Centros.cshtml");
            }
        }
        public ActionResult EnviarCodigoRecuperacionContrasenya(string Email)
        {
            int error = 0;
            string msg = "Código de recuperación enviado";
            using (var db = new NutribuddyEntities())
            {
                Usuario usuario = (from u in db.Usuario where u.Email == Email && u.Activo select u).FirstOrDefault();

                if (usuario != null)
                {
                    string codigoRecuperacion = Utilidades.Seguridad.ObtenerCodigoRecuperacion();

                    usuario.CodigoRecuperacion = codigoRecuperacion;
                    usuario.FechaCodigoRecuperacion = DateTime.Now.AddHours(1);
                    db.Entry(usuario).State = System.Data.Entity.EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                        error = Utilidades.Email.EnviarEmail("Código de recuperación NutriBuddy", "Content\\Templates\\EmailCodigoRecuperacion-es.html", new Dictionary<string, string> { { "##CODE##", codigoRecuperacion } }, Email);
                        switch (error)
                        {
                            case 1: //formato de email inválido
                                msg = "Introduzca un email válido";
                                break;
                            case 2: //error enviando email
                                msg = "Ha ocurrido un error enviando el email. Intentelo más tarde";
                                break;
                        }
                    }
                    catch
                    {
                        error = 4;
                        msg = "Ha ocurrido un error enviando el email. Intentelo más tarde";
                    }

                }
                else
                {
                    error = 3;
                    msg = "No hay ninguna cuenta asociada a este email";
                }
            }


            return Json(new { flag = error, msg = msg });
        }

        public ActionResult CambiarContrasenya(FormCambioContrasenya datos)
        {
            int error = 0;
            string msg = "Contraseña cambiada con éxito";
            using (var db = new NutribuddyEntities())
            {
                if (datos.NuevaContrasenya.Length >= 8)
                {
                    Usuario usuario = (from u in db.Usuario where u.Email == datos.Email && u.Activo select u).FirstOrDefault();
                    if (usuario != null)
                    {
                        if (usuario.CodigoRecuperacion == datos.CodigoRecuperacion)
                        {
                            if (usuario.FechaCodigoRecuperacion != null && DateTime.Now <= usuario.FechaCodigoRecuperacion)
                            {
                                usuario.Password = Utilidades.Seguridad.EncriptacionAES(datos.NuevaContrasenya, usuario.FechaAlta.ToString(new System.Globalization.CultureInfo("es-ES")));
                                usuario.FechaCodigoRecuperacion = null;
                                db.Entry(usuario).State = System.Data.Entity.EntityState.Modified;
                                try
                                {
                                    db.SaveChanges();
                                }
                                catch
                                {
                                    error = 5;
                                    msg = "Ha ocurrido un error al cambiar la contraseña. Intentelo más tarde";
                                }
                            }
                            else
                            {
                                error = 4;
                                msg = "Código de recuperación caducado";
                            }
                        }
                        else
                        {
                            error = 2;
                            msg = "Código de recuperación incorrecto";
                        }

                    }
                    else
                    {
                        error = 3;
                        msg = "No hay ninguna cuenta asociada a este email";
                    }
                }
                else
                {
                    error = 1;
                    msg = "La contraeña debe ser igual o mayor a 8 carácteres";
                }
            }
            return Json(new { flag = error, msg = msg });
        }

        public ActionResult Registrar(FormRegistrar datos)
        {
            int error = 0;
            string msg = "Registo éxitoso";
            using (var db = new NutribuddyEntities())
            {
                Usuario usuario = new Usuario();
                usuario.IDRol = 5;
                usuario.Email = datos.Email;
                usuario.FechaAlta = DateTime.Now;
                usuario.Password = Utilidades.Seguridad.EncriptacionAES(datos.Contrasenya, usuario.FechaAlta.ToString(new System.Globalization.CultureInfo("es-ES"))); ;
                usuario.Activo = true;
                usuario.IDProvincia = datos.Provincia;
                usuario.Nombre = datos.Nombre;
                usuario.Apellidos = datos.Apellidos;

                db.Usuario.Add(usuario);
                try
                {
                    db.SaveChanges();
                    DateTime fAux = DateTime.Parse(datos.FechaNacimiento);
                    db.Paciente.Add(new Paciente()
                    {
                        FechaNacimiento = fAux,
                        Direccion = datos.Direccion,
                        DNI = datos.Dni,
                        Telefono = datos.Telefono,
                        IDUsuario = usuario.ID,
                        EsHombre = datos.EsHombre,
                        IDCentro = datos.Centro == 0 ? (int?)null : datos.Centro
                    });
                    db.SaveChanges();
                }
                catch
                {
                    error = 1;
                    msg = "Ha ocurrido un error registrando sus datos. Intentelo más tarde";
                }
            }
            return Json(new { flag = error, msg = msg });
        }

        public ActionResult EmailEnBBDD(string Email)
        {
            int error = 0;
            bool emailEnBBDD = false;
            using (var db = new NutribuddyEntities())
            {
                try
                {
                    emailEnBBDD = (from u in db.Usuario where u.Email == Email && u.Activo select u.Email).Any();
                }
                catch (Exception)
                {
                    error = 1;
                }
            }

            return Json(new { flag = error, emailEnBBDD = emailEnBBDD });
        }

        public ActionResult ObtenerCentros(int IdProvincia)
        {
            int error = 0;
            String jsonCentros = "";
            Dictionary<int, string> centros = null;

            using (var db = new NutribuddyEntities())
            {
                try
                {
                    centros = (from c in db.Centro where c.IDProvincia == IdProvincia && c.Activo select new CentroBasico() { ID = c.ID, Nombre = c.NombreCentro }).OrderBy(x => x.Nombre).ToDictionary(x => x.ID, x => x.Nombre);
                }
                catch (Exception)
                {
                    error = 1;
                }
            }

            if (centros != null)
            {
                if (centros.Count > 0)
                {
                    jsonCentros = new JavaScriptSerializer().Serialize(centros.ToDictionary(item => item.Key.ToString(), item => item.Value.ToString()));

                }
                else
                {
                    error = 2;
                }

            }

            return Json(new { flag = error, centros = jsonCentros });
        }


    }


}
