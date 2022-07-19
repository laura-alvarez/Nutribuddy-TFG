using NutriBuddy.Models;
using NutriBuddy.Models.Propios;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.Mvc;

namespace NutriBuddy.Controllers
{
    public class ZonaPrivadaController : ParentController
    {
        // GET: Administration
        public ActionResult Home()
        {
            var permisos = Session["permisos"] as List<int>;
            if (permisos != null && permisos.Count > 0)
            {
                Session["seccionActiva"] = permisos[0];
                return ZonaPrivada(permisos[0]);
            }
            return CerrarSesion();
        }

        public ActionResult ZonaPrivada(int menu)
        {            
            var idioma = Session["idioma"].ToString();
            var permisos = Session["permisos"] as List<int>;
            if (!permisos.Contains(menu)) return CerrarSesion();
            Session["seccionActiva"] = menu;
            string ruta = "";
            RUTAS.TryGetValue(menu, out ruta);
            ViewData["estadoMenu"] = GetEstadoMenu();
            
            if (!string.IsNullOrEmpty(ruta))
            {
                using (var db = new NutribuddyEntities())
                {
                    Usuario usuario = Session["usuario"] as Usuario;
                    Session["mostrarCentros"] = usuario.IDRol == 3 || usuario.IDRol == 4;
                    var centros = (from tc in db.CentroTrabajador where tc.IDTrabajador == usuario.ID select new Basico()
                    {
                        ID = tc.IDCentro,
                        Nombre = tc.Centro.NombreCentro
                    }).ToList();
                    Session["centros"] = centros == null ? new List<Basico>() : centros;
                    if (Session["centroActual"] == null)
                    {
                        Session["centroActual"] = (centros == null || (centros != null && centros.Count == 0)) ? -1 : centros.FirstOrDefault().ID;
                    }
                    switch (menu)
                    {
                        case 1:
                            break;
                        case 1003: //HOME DE LA ZONA PRIVADA 
                            ViewData["mes"] = _MesTexto();
                            if(usuario.IDRol == 5)
                            {
                                DateTime ahora = DateTime.Now;
                                //BUSCAMOS SI TIENE UNA CITA PROXIMA
                                var cita = (from c in db.Cita where c.IDPaciente == usuario.ID && c.Activo && c.HoraInicio >= DateTime.Now select c).OrderBy(x => x.HoraInicio).FirstOrDefault();
                                ViewData["hayCita"] = cita != null;
                                if (cita != null)
                                {
                                    ViewData["sigCitaCentro"] = cita.Centro.NombreCentro.ToUpper();
                                    ViewData["sigCitaDia"] = cita.HoraInicio.ToString("g");
                                    ViewData["sigCitaTrabajador"] = cita.Trabajador.Usuario.Nombre.ToUpper()+" "+ cita.Trabajador.Usuario.Apellidos.ToUpper()+ " - "+cita.Puesto.Nombre;
                                }
                                else
                                {
                                    ViewData["sigCitaCentro"] = "";
                                    ViewData["sigCitaDia"] = "";
                                    ViewData["sigCitaTrabajador"] = "";
                                }
                            }
                            break;
                        case 4: //PERFIL DEL PACIENTE
                            Dictionary<string, List<ProvinciaBasica>> localizaciones = new Dictionary<string, List<ProvinciaBasica>>();
                            var ccaa = (from ca in db.ComunidadAutonoma select ca).OrderBy(x => x.Nombre).ToList();
                            foreach (ComunidadAutonoma ca in ccaa)
                            {
                                var provincias = (from p in db.Provincia where p.IDCCAA == ca.ID select new ProvinciaBasica() { ID = p.ID, Nombre = p.Nombre }).OrderBy(x => x.Nombre).ToList();
                                localizaciones.Add(ca.Nombre, provincias);
                            }
                            ViewData["localizaciones"] = localizaciones;                            
                            
                            Paciente paciente = (from p in db.Paciente where p.IDUsuario == usuario.ID select p).FirstOrDefault();
                            string nombreProvincia = (from p in db.Provincia where p.ID == usuario.IDProvincia select p.Nombre).Single();

                            ViewData["datosPerfil"] = new Dictionary<string, string>() {
                                { "Nombre", usuario.Nombre },
                                { "Apellidos", usuario.Apellidos },
                                { "Email", usuario.Email },
                                { "DNI", paciente.DNI },
                                { "Direccion", paciente.Direccion },
                                { "Provincia", nombreProvincia},
                                { "Telefono", paciente.Telefono },
                                { "FechaNacimiento", paciente.FechaNacimiento.ToShortDateString() }, //Falta añadir un IFormatProvider para su representación culturar correcta Session["idioma"] solo devuelve "es" en vez de "es-ES"
                            };
                            
                            break;

                        case 5: //AGENDA
                            int idCentro = int.Parse(Session["centroActual"].ToString());
                            var idTrabajadores = (from tc in db.Trabajador join
                                                 c in db.CentroTrabajador on tc.IDUsuario equals c.IDTrabajador
                                                 where tc.Activo && tc.Activo && c.IDCentro == idCentro 
                                                select tc.IDUsuario).ToList();
                            var puestosCentro = (from p in db.PuestoTrabajador where idTrabajadores.Contains(p.IDUsuario)
                                                 && p.Activo select p.IDPuesto).ToList();
                            ViewData["puestos"] = (from p in db.Puesto
                                                   where puestosCentro.Contains(p.ID) 
                                                   select new Models.Propios.Puesto() {
                                                        IDPuesto = p.ID,
                                                        Nombre = p.Nombre,
                                                        Color = p.colorAgenda
                                                    }).ToList();

                            ViewData["trabajadores"] = (from tc in db.CentroTrabajador
                                                join t in db.Trabajador on tc.IDTrabajador equals t.IDUsuario
                                                where tc.IDCentro == idCentro && t.Activo && tc.Activo
                                                select new Basico() { 
                                                    ID = t.IDUsuario,
                                                    Nombre = t.Usuario.Nombre+" "+t.Usuario.Apellidos
                                                }).ToList();
                            
                            break;
                        case 9:
                              if(idioma == "en")
                            {
                                var emociones = (from e in db.EstadoAnimicoTipo
                                             select new Basico()
                                             {
                                                 ID = e.ID,
                                                 Nombre = e.Nombre+"_ic.svg",
                                                 NombreEN = e.NombreEN
                                             }).ToList();
                                ViewData["emociones"] = emociones;
                                var deportes = (from d in db.DeporteTipo
                                                select new Basico()
                                                {
                                                    ID = d.ID,
                                                    Nombre = d.ID+".png",
                                                    NombreEN = d.NombreEN
                                                }).ToList();
                                ViewData["deportes"] = deportes;                                
                            }
                            else
                            {
                                var emociones = (from e in db.EstadoAnimicoTipo
                                 select new Basico()
                                 {
                                     ID = e.ID,
                                     Nombre = e.Nombre,
                                     NombreEN = e.Nombre + "_ic.svg",
                                 }).ToList();
                                ViewData["emociones"] = emociones;
                                var deportes = (from d in db.DeporteTipo
                                                select new Basico()
                                                {
                                                    ID = d.ID,
                                                    Nombre = d.Nombre,
                                                    NombreEN = d.ID + ".png"
                                                }).ToList();
                                ViewData["deportes"] = deportes;                               
                            }
                            var esHombre = (from p in db.Paciente where p.IDUsuario == usuario.ID select p.EsHombre).FirstOrDefault();
                            ViewData["esHombre"] = esHombre;
                            var tipo = (from t in db.OpcionesDiarioTipo
                                        select new Basico()
                                        {
                                            ID = t.ID,
                                            Nombre = t.Nombre,
                                            NombreEN = t.NombreEN
                                        }).ToList();
                            ViewData["tipo"] = tipo;

                            break;
                        case 1002:
                            var centro = (from c in db.Centro
                                          where c.ID == usuario.Paciente.IDCentro && c.Activo
                                          select new CentroForm()
                                          {
                                              IDCentro = c.ID,
                                              Nombre = c.NombreCentro,
                                              Descripcion = c.Descripcion,
                                              Telefono = c.Telefono,
                                              Direccion = c.Direccion,
                                              Email = c.Email,
                                              Consultation_F2F = c.EsPresencial,
                                              Consultation_Online = c.EsOnline
                                          }).FirstOrDefault();
                            ViewData["centro"] = centro;
                            break;
                        
                    }
                    return View(ruta);
                }
            }
            return CerrarSesion();
        }

        private string _MesTexto()
        {
            switch (DateTime.Now.Month)
            {
                case 1: return Resources.mesesAnyo._1;
                case 2: return Resources.mesesAnyo._2;
                case 3: return Resources.mesesAnyo._3;
                case 4: return Resources.mesesAnyo._4;
                case 5: return Resources.mesesAnyo._5;
                case 6: return Resources.mesesAnyo._6;
                case 7: return Resources.mesesAnyo._7;
                case 8: return Resources.mesesAnyo._8;
                case 9: return Resources.mesesAnyo._9;
                case 10: return Resources.mesesAnyo._10;
                case 11: return Resources.mesesAnyo._11;
                case 12: default: return Resources.mesesAnyo._12;
            }
        }
         
        public void ActualizarEstadoMenu(bool Collapse)
        {
            HttpCookie ckMenu = Request.Cookies["collapse"];
            if (ckMenu == null)
            {
                ckMenu = new HttpCookie("collapse", Collapse.ToString());
            }
            else
            {
                ckMenu.Value = Collapse.ToString();
            }
            ckMenu.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Add(ckMenu);
        }

        public bool GetEstadoMenu()
        {
            bool Collapse = false;
            HttpCookie ckMenu = Request.Cookies["collapse"];
            if (ckMenu != null)
            {
                Collapse = Boolean.Parse(ckMenu.Value);
            }
            return Collapse;
        }

        public ActionResult CerrarSesion()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CambiarCentro(int IDCentro)
        {
            Session["centroActual"] = IDCentro;
            return Json(new { },JsonRequestBehavior.AllowGet);

        }

        #region Home Paciente de la Zona Privada
        public ActionResult GetDatosResumenPaciente()
        {
            
            Usuario usuario = Session["usuario"] as Usuario;
            if (_HayDiarioMes(usuario.ID))
            {
                ProPeso progresoPeso = _GetProgresoPeso(usuario.ID);
                int diasDeporte = _GetDiasDeporte(usuario.ID);
                Basico emocion = _GetEmocion(usuario.ID);
                Basico suenyo = _GetSuenyo(usuario.ID);
                return Json(new { flag=0, datosG = progresoPeso, diasDeporte = diasDeporte, emocion = emocion, suenyo = suenyo }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { flag = 1 }, JsonRequestBehavior.AllowGet);
        }

        private bool _HayDiarioMes(int idUsuario)
        {
            using(var db= new NutribuddyEntities())
            {
                var boo = (from d in db.Diario where d.Activo && d.IDPaciente == idUsuario && d.Dia.Month == DateTime.Now.Month && d.Dia.Year == DateTime.Now.Year select d).Any();
                return boo;
            }
        }
        public class ProPeso
        {
            public string[] Etiquetas { get; set; }
            public double[] Datos { get; set; }

            public ProPeso(string[] etiquetas, double[] datos)
            {
                this.Etiquetas = etiquetas;
                this.Datos = datos;
            }
        }
        private ProPeso _GetProgresoPeso(int idUsuario)
        {
            using(var db= new NutribuddyEntities())
            {
                DateTime fFin = DateTime.Today;
                DateTime fInicio = new DateTime(fFin.Year, fFin.Month, 1);
                string[] etiquetas = new string[fFin.Day];
                double[] datos = new double[fFin.Day];
                double ultimoDato = 0d;
                while (fInicio <= fFin)
                {
                    etiquetas[fInicio.Day - 1] = fInicio.ToString("dd-MM");
                    decimal? mPeso = (from m in db.PacienteMedidas where m.PostedDate == fInicio && m.Activo && m.IDPaciente == idUsuario select m.Peso).FirstOrDefault();
                    if(mPeso != null)
                    {
                        ultimoDato = (double)mPeso;
                        
                    }
                    datos[fInicio.Day - 1] = ultimoDato;
                    fInicio = fInicio.AddDays(1);
                }

                return new ProPeso(etiquetas, datos);                
                
            }
        }

        private int _GetDiasDeporte(int idUsuario)
        {
            int mes = DateTime.Now.Month;
            int anyo = DateTime.Now.Year;
            using(var db= new NutribuddyEntities())
            {
                var diasDeporte = (from d in db.Diario where d.IDPaciente == idUsuario && d.Dia.Month == mes && d.Dia.Year == anyo && d.Activo && d.Deporte select d).Count();
                return diasDeporte;
            }
        }

        private Basico _GetEmocion(int idUsuario)
        {
            int mes = DateTime.Today.Month;
            int anyo = DateTime.Today.Year;
            using (var db = new NutribuddyEntities())
            {
                var idioma = Session["idioma"].ToString();
                var idDiarioMes = (from d in db.Diario where d.Dia.Month == mes && d.Dia.Year == anyo && d.IDPaciente == idUsuario && d.Activo select d.ID).ToList();
                var resultado = (from e in db.EstadoAnimicoDiario
                              where idDiarioMes.Contains(e.IDDiario)
                              group e by e.IDEstadoAnimico into g
                              orderby g.Count() descending
                              select new Basico
                              {
                                  ID = g.Key,
                                  Nombre = (from em in db.EstadoAnimicoTipo where em.ID == g.Key select em.Nombre).FirstOrDefault(),
                                  NombreEN = (from em in db.EstadoAnimicoTipo where em.ID == g.Key select em.NombreEN).FirstOrDefault()
                              }).FirstOrDefault();
                if (resultado != null)
                {
                    resultado.NombreEN = resultado.Nombre;
                    resultado.Nombre = idioma == "es" ? resultado.Nombre.ToLower() : resultado.NombreEN.ToLower();
                }
                return resultado;
            }
        }

        private Basico _GetSuenyo(int idUsuario)
        {
            int mes = DateTime.Today.Month;
            int anyo = DateTime.Today.Year;
            using (var db = new NutribuddyEntities())
            {
                var resultado =  (from d in db.Diario
                        where d.Dia.Month == mes && d.Dia.Year == anyo && d.IDPaciente == idUsuario 
                        group d by d.IDSuenyo into g
                        orderby g.Count() descending
                        select new Basico
                        {
                            ID = g.Key == null?2:(int)g.Key                           
                        }).FirstOrDefault();
                if (resultado != null)
                {
                    var idioma = Session["idioma"].ToString();
                    switch (resultado.ID)
                    {
                        case 3: //Mal
                            resultado.Nombre = Resources.paciente.pz_suenyo_mal;
                            break;
                        case 1: //Bien
                            resultado.Nombre = Resources.paciente.pz_suenyo_bien;
                            break;
                        case 2: //Regular
                        default:
                            resultado.Nombre = Resources.paciente.pz_suenyo_bien;
                            break;
                    }
                }
                else
                {
                    resultado = new Basico() {Nombre = Resources.paciente.pz_suenyo_404 };
                }
                return resultado;
            }
        }
        #endregion
    }
}