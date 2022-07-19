using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NutriBuddy.Models;
using NutriBuddy.Models.Propios;


namespace NutriBuddy.Controllers.Utilidades
{
    public class CentrosController : ParentController
    {
        public ActionResult ListadoCentros()
        {
            return View();
        }
       
        public ActionResult GetCentros()
        {
            using(var db = new NutribuddyEntities())
            {
                var centros = (from c in db.Centro where c.Activo
                               select new CentroListado() { 
                                    ID = c.ID,
                                    Nombre = c.NombreCentro,
                                    Plan = c.Plan.Nombre,
                                    Provincia = c.Provincia.Nombre,
                                    Email = c.Email,
                                    Telefono = c.Telefono,
                                    Responsable = c.Usuario.Nombre+" "+c.Usuario.Apellidos
                                    }).ToList();
                return Json(new { Centros = centros }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult FichaCentro(int id)
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

                var planes = (from p in db.Plan
                              where p.Activo
                              select new Planes()
                              {
                                  ID = p.ID,
                                  Nombre = p.Nombre
                              }).ToList();

                ViewData["planes"] = planes;

                var puestos = (from p in db.Puesto
                               select new NutriBuddy.Models.Propios.Puesto()
                               {
                                   IDPuesto = p.ID,
                                   Nombre = p.Nombre
                               }).ToList();
                ViewData["puestos"] = puestos;
                ViewData["idCentro"] = id;
                if (id > 0)
                {
                    var centro = (from c in db.Centro where c.ID == id 
                                  select new CentroForm() { 
                                    Nombre = c.NombreCentro,
                                    CIF = c.CIF,
                                    Descripcion = c.Descripcion,
                                    IDPlan = c.IDPlan,
                                    Direccion = c.Direccion,
                                    IDProvincia = c.IDProvincia,
                                    Localidad = c.Localidad,
                                    CP = c.CP,
                                    Telefono = c.Telefono,
                                    Email = c.Email,
                                    Website = c.Website,
                                    Consultation_F2F = c.EsPresencial,
                                    Consultation_Online = c.EsOnline,
                                    MaxTrabajadores = c.MaxTrabajadores,
                                    TrabajadoresActuales = (from t in db.CentroTrabajador
                                                            where t.Activo && t.IDCentro == c.ID
                                                            select t).Count()
                                  }).FirstOrDefault();
                    var idR = (from c in db.Centro
                               where c.ID == id
                               select c.IDUsuarioAdminitrador).FirstOrDefault();
                    var responsable = (from t in db.Trabajador
                                       join u in db.Usuario
                                       on t.IDUsuario equals u.ID
                                       where u.ID == idR
                                       select new TrabajadorCForm() {
                                           Nombre = u.Nombre,
                                           Apellidos = u.Apellidos,
                                           SS = t.NumSegSocial,
                                           DNI = t.DNI,
                                           IDProvincia = u.IDProvincia,
                                           Email = u.Email,
                                           Telefono = t.Telefono,
                                           Localidad = t.Localidad,
                                           Nacionalidad = t.Nacionalidad,
                                           Puestos = (from p in db.PuestoTrabajador 
                                            where p.IDUsuario == u.ID && p.Activo 
                                            select p.IDPuesto).ToList()
                                       }).FirstOrDefault();
                    ViewData["centro"] = centro;
                    ViewData["trabajador"] = responsable;
                }
                return View("~/Views/Centros/FichaCentro.cshtml");
            }
        }

        public ActionResult GuardarCentro(CentroForm centro)
        {
            int flag = 0;
            string msg;
            using (var db = new NutribuddyEntities())
            {
                if (centro.IDCentro == 0)
                {
                    DateTime fecha = DateTime.Now;
                    byte[] encrypted = Seguridad.EncriptacionAES("admin", fecha.ToString(new System.Globalization.CultureInfo("es-ES")));
                    var usuario = db.Usuario.Add(new Usuario()
                    {
                        Activo = true,
                        Nombre = centro.NombreR,
                        Apellidos = centro.ApellidosR,
                        Email = centro.EmailR,
                        IDProvincia = centro.IDProvinciaR,
                        FechaAlta = fecha,
                        Password = encrypted,
                        IDRol= 3
                    });                   
                    var centroDB = db.Centro.Add(new Centro()
                    {
                        Activo = true,
                        NombreCentro = centro.Nombre,
                        CIF = centro.CIF,
                        Descripcion = centro.Descripcion ?? "",
                        IDPlan = centro.IDPlan,
                        Direccion = centro.Direccion,
                        IDProvincia = centro.IDProvincia,
                        Localidad = centro.Localidad,
                        CP = centro.CP,
                        Telefono = centro.Telefono,
                        Email = centro.Email,
                        Website = centro.Website,
                        EsPresencial = centro.Consultation_F2F,
                        EsOnline = centro.Consultation_Online,
                        IDUsuarioAdminitrador = usuario.ID,                        
                        FechaAlta = DateTime.Now,
                        MaxTrabajadores = centro.MaxTrabajadores,
                        TrabajadoresActuales = 1 //MINIMO EL RESPONSABLES CON ROL ADMINISTRATIVO
                    });
                    db.Trabajador.Add(new Trabajador()
                    {
                        Activo = true,
                        IDUsuario = usuario.ID,
                        DNI = centro.DNIR,
                        Localidad = centro.LocalidadR,
                        Nacionalidad = centro.NacionalidadR,
                        Telefono = centro.TelefonoR,
                        NumSegSocial = centro.SSR,
                        FechaAlta = DateTime.Now
                    });

                    db.PuestoTrabajador.Add(new PuestoTrabajador()
                    {
                        Activo = true,
                        IDCentro = centroDB.ID,
                        IDPuesto = 1, //Administrativo
                        IDUsuario = usuario.ID
                    });
                    msg = "Centro dado de alta";
                }
                else
                {
                    var centroBD = db.Centro.Find(centro.IDCentro);

                    centroBD.NombreCentro = centro.Nombre;
                    centroBD.CIF = centro.CIF;
                    centroBD.Descripcion = centro.Descripcion;
                    if(centroBD.IDPlan != centro.IDPlan)
                    {
                        centroBD.MaxTrabajadores = (from plan in db.Plan where plan.ID == centro.IDPlan select plan.MaxTrabajadores).FirstOrDefault();
                    }
                    centroBD.IDPlan = centro.IDPlan;

                    centroBD.Direccion = centro.Direccion;
                    centroBD.IDProvincia = centro.IDProvincia;
                    centroBD.Localidad = centro.Localidad;
                    centroBD.CP = centro.CP;
                    centroBD.Telefono = centro.Telefono;
                    centroBD.Email = centro.Email;
                    centroBD.Website = centro.Website;
                    centroBD.EsPresencial = centro.Consultation_F2F;
                    centroBD.EsOnline = centro.Consultation_Online;

                    var usuario = db.Usuario.Find(centroBD.Usuario.ID);
                    var trabajador = db.Trabajador.Find(usuario.ID);

                    usuario.Nombre = centro.NombreR;
                    usuario.Apellidos = centro.ApellidosR;
                    usuario.Email = centro.EmailR;
                    usuario.IDProvincia = centro.IDProvinciaR;

                    trabajador.NumSegSocial = centro.SSR;
                    trabajador.DNI = centro.DNIR;
                    trabajador.Nacionalidad = centro.NacionalidadR;
                    trabajador.Localidad = centro.LocalidadR;
                    trabajador.Telefono = centro.TelefonoR;
    

                    db.Entry(centroBD).State = System.Data.Entity.EntityState.Modified;
                    db.Entry(usuario).State = System.Data.Entity.EntityState.Modified;
                    db.Entry(trabajador).State = System.Data.Entity.EntityState.Modified;
                    msg = "Centro actualizado";
                }

                try { db.SaveChanges(); }catch(Exception e) { System.Diagnostics.Debug.WriteLine(e.Message); flag = 1; msg = "Error, por favor intentelo más tarde"; }
                return Json(new { flag = flag, msg = msg });
            }
        }

        [HttpDelete]
        public ActionResult EliminarCentro(int id)
        {
            using (var db = new NutribuddyEntities())
            {
                var centro = db.Centro.Find(id);
                if(centro != null)
                {
                    centro.Activo = false;
                    centro.FechaBaja = DateTime.Now;
                    db.Entry(centro).State = System.Data.Entity.EntityState.Modified;
                    var trabajadoresCentro = (from ct in db.CentroTrabajador where ct.Activo && ct.IDCentro == id select ct).ToList();
                    foreach(CentroTrabajador t in trabajadoresCentro)
                    {
                        t.Activo = false;
                        db.Entry(t).State = System.Data.Entity.EntityState.Modified;

                        //SI NO ES TRABAJADOR DE NINGÚN CENTRO MÁS SE DA DE BAJA SU USUARIO
                        Trabajador tra = db.Trabajador.Find(t.IDTrabajador);
                        int nCentros = (from tC in db.CentroTrabajador where tC.IDTrabajador == tra.IDUsuario && tC.Activo select tC).Count();
                        if(nCentros <= 1)
                        {
                            tra.Activo = false;
                            tra.FechaBaja = DateTime.Now;
                            tra.Usuario.Activo = false;
                            tra.Usuario.FechaBaja = DateTime.Now;
                            db.Entry(tra).State = System.Data.Entity.EntityState.Modified;

                            var puestos = (from p in db.PuestoTrabajador where p.IDUsuario == tra.IDUsuario && p.Activo && p.IDCentro == id select p).ToList();
                            foreach(PuestoTrabajador p in puestos)
                            {
                                p.Activo = false;
                                db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                            }
                        } 
                    }
                    db.SaveChanges();
                }
                return Json(new { flag = 0, msg = "Centro dado de baja" });
            }
        }

        public ActionResult GetTrabajadoresActuales(int idCentro, int idPlan)
        {
            using(var db= new NutribuddyEntities())
            {
                var numTrabajadores = (from c in db.Centro where c.ID == idCentro select c.TrabajadoresActuales).FirstOrDefault();
                var plan = (from p in db.Plan where p.ID == idPlan select p.MaxTrabajadores).FirstOrDefault();
                var resta = plan - numTrabajadores;
                return Json(new {resta = resta },JsonRequestBehavior.AllowGet);
            }
        }
    }
}