using NutriBuddy.Controllers.Utilidades;
using NutriBuddy.Models;
using NutriBuddy.Models.Propios;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NutriBuddy.Controllers
{
    public class PersonalController : ParentController
    {
        // GET: Personal
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetTrabajadores()
        {
            Usuario usuario = Session["usuario"] as Usuario;
            using(var db=  new NutribuddyEntities())
            {
                List<TrabajadorNB> trabajadores = null;
                if(usuario.IDRol == 1)
                {
                    trabajadores = (from t in db.TrabajadoresNB
                                    where t.Activo
                                    select new TrabajadorNB()
                                    {
                                        Nombre = t.Usuario.Nombre,
                                        Apellidos = t.Usuario.Apellidos,
                                        Email = t.Usuario.Email,
                                        Provincia = t.Usuario.Provincia.Nombre,
                                        ID = t.IDUsuario
                                    }).ToList();
                }
                else
                {
                    int idCentro = int.Parse(Session["centroActual"].ToString());
                    trabajadores = (from t in db.Trabajador
                                    join ct in db.CentroTrabajador
                                    on t.IDUsuario equals ct.IDTrabajador
                                    where t.Activo && ct.Activo &&  ct.IDCentro == idCentro
                                    select new TrabajadorNB()
                                    {
                                        Nombre = t.Usuario.Nombre,
                                        Apellidos = t.Usuario.Apellidos,
                                        Email = t.Usuario.Email,
                                        Provincia = t.Usuario.Provincia.Nombre,
                                        ID = t.IDUsuario
                                    }).ToList();
                }
                return Json(new { Trabajadores = trabajadores}, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EliminarEmpleado(int id)
        {
            int bandera = 0;
            string msg = "Empleado dado de baja";
            Usuario usuario = Session["usuario"] as Usuario;
            using (var db = new NutribuddyEntities())
            {
                if (usuario.IDRol == 1)
                {
                    var trabajador = (from t in db.TrabajadoresNB
                                      where t.Activo && t.IDUsuario == id
                                      select t).FirstOrDefault();
                    if (trabajador != null)
                    {
                        trabajador.Activo = false;
                        trabajador.FechaBaja = DateTime.Now;
                        trabajador.Usuario.Activo = false;
                        trabajador.Usuario.FechaBaja = DateTime.Now;

                        db.Entry(trabajador).State = System.Data.Entity.EntityState.Modified;

                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            bandera = 1;
                            msg = "¡Ups! Ha ocurrido un error, intentelo más tarde";
                        }

                    }
                }
                else
                {
                    var trabajador = (from t in db.Trabajador
                                      where t.Activo && t.IDUsuario == id
                                      select t).FirstOrDefault();
                    if (trabajador != null)
                    {
                        trabajador.Activo = false;
                        trabajador.FechaBaja = DateTime.Now;
                        trabajador.Usuario.Activo = false;
                        trabajador.Usuario.FechaBaja = DateTime.Now;

                        db.Entry(trabajador).State = System.Data.Entity.EntityState.Modified;

                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            bandera = 1;
                            msg = "¡Ups! Ha ocurrido un error, intentelo más tarde";
                        }
                    }
                }
            }
            return Json(new { flag = bandera, msg = msg });
        }

        public ActionResult GUsuario(int id)
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
                               select new NutriBuddy.Models.Propios.Puesto()
                               {
                                   IDPuesto = p.ID,
                                   Nombre = p.Nombre
                               }).ToList();
                ViewData["puestos"] = puestos;
                ViewData["origen"] = 2;
                Usuario usuario = Session["usuario"] as Usuario;
                ViewData["esDeNB"] = usuario.IDRol == 1;
                ViewData["idTrabajador"] = id;
                if (id > 0)
                {
                    if (usuario.IDRol == 1)
                    {
                        TrabajadorCForm trabajadorNB = (from t in db.TrabajadoresNB
                                                     where t.Activo && t.IDUsuario == id
                                                     select new TrabajadorCForm()
                                                     {
                                                         ID = t.IDUsuario,
                                                         Nombre = t.Usuario.Nombre,
                                                         Apellidos = t.Usuario.Apellidos,
                                                         Email = t.Usuario.Email,
                                                         IDProvincia = t.Usuario.IDProvincia,
                                                         SS = t.NumSegSocial
                                                     }).FirstOrDefault();
                        ViewData["trabajador"] = trabajadorNB;
                    }
                    else
                    {
                        int idCentro = int.Parse(Session["centroActual"].ToString());
                        TrabajadorCForm trabajadorNB = (from t in db.Trabajador
                                                     where t.Activo && t.IDUsuario == id 
                                                     select new TrabajadorCForm()
                                                     {
                                                         ID = t.IDUsuario,
                                                         Nombre = t.Usuario.Nombre,
                                                         Apellidos = t.Usuario.Apellidos,
                                                         Email = t.Usuario.Email,
                                                         IDProvincia = t.Usuario.IDProvincia,
                                                         SS = t.NumSegSocial,
                                                         DNI = t.DNI,
                                                         Localidad = t.Localidad,
                                                         Nacionalidad = t.Nacionalidad,
                                                         Telefono = t.Telefono,
                                                         Puestos = (from p in db.PuestoTrabajador where p.Activo && p.IDUsuario == id && p.IDCentro == idCentro select p.IDPuesto).ToList()                                                         
                                                     }).FirstOrDefault();
                        ViewData["trabajador"] = trabajadorNB;
                    }
                }
                return View();
            }
        }

        public ActionResult GuardarTrabajadorNB(TrabajadorNB trabajador)
        {
            int bandera = 0;
            string msg = trabajador.ID == 0 ? "Trabajador dado de alta correctamente" : "Datos del trabajador actualizado correctamente";
            using (var db = new NutribuddyEntities())
            {
                if (trabajador.ID == 0)
                {
                    //TRABAJADOR NUEVO SE AÑADE COMO PACIENTE TAMBIÉN
                    var usuario = new Usuarios()
                    {
                        Nombre = trabajador.Nombre,
                        Apellidos = trabajador.Apellidos,
                        Email = trabajador.Email,
                        IDProvincia = trabajador.IDProvincia,
                        IDRol = 2
                    };
                    UsuarioController.CrearUsuario(ref usuario);
                    if (usuario.ID > 0)
                    {
                        var trNB = db.TrabajadoresNB.Add(new TrabajadoresNB()
                        {
                            IDUsuario = usuario.ID,
                            NumSegSocial = trabajador.SSR,
                            FechaAlta = DateTime.Now,
                            Activo = true
                        });
                        try
                        {
                            db.SaveChanges();
                            //SE ENVÍA EMAIL DE BIENVENIDA CON DATOS DE ACCECSO
                            Dictionary<string, string> datos = new Dictionary<string, string>();
                            datos.Add("##NOMBRECOMPLETO##", usuario.Nombre.ToUpper() + " " + usuario.Apellidos.ToUpper());
                            datos.Add("##EMAIL##", usuario.Email);
                            datos.Add("##CONTRASENYA##", usuario.Password);
                            Email.EnviarEmail("Bienvenido a la familia de NutriBuddy", "Content\\Templates\\NuevoTNB.html", datos);
                        }
                        catch (Exception e)
                        {
                            bandera = 1;
                            msg = "Error guardando los datos del trabajador";
                        }
                    }
                    else
                    {
                        bandera = 1;
                        msg = "Error guardando los datos del trabajador";
                    }
                }
                else
                {
                    var tNB = (from t in db.TrabajadoresNB where t.Activo && t.IDUsuario == trabajador.ID select t).FirstOrDefault();
                    tNB.IDUsuario = trabajador.ID;
                    tNB.NumSegSocial = trabajador.SSR;
                    db.Entry(tNB).State = System.Data.Entity.EntityState.Modified;
                    var usu = (from u in db.Usuario where u.Activo && u.ID == trabajador.ID select u).FirstOrDefault();
                    usu.Nombre = trabajador.Nombre;
                    usu.Apellidos = trabajador.Apellidos;
                    usu.Email = trabajador.Email;
                    usu.IDProvincia = trabajador.IDProvincia;
                    db.Entry(usu).State = System.Data.Entity.EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        bandera = 1;
                        msg = "Error guardando los datos del trabajador";
                    }

                }
                return Json(new { flag = bandera, msg = msg });
            }
        }

        public ActionResult GuardarTrabajadorC(TrabajadorCForm trabajador)
        {
            int bandera = 0;
            string msg = trabajador.ID == 0 ? "Trabajador dado de alta correctamente" : "Datos del trabajador actualizado correctamente";
            using (var db = new NutribuddyEntities())
            {
                int idCentro = int.Parse(Session["centroActual"].ToString());
                if (trabajador.ID == 0)
                {
                    //TRABAJADOR NUEVO SE AÑADE COMO PACIENTE TAMBIÉN
                    var usuario = new Usuarios()
                    {
                        Nombre = trabajador.Nombre,
                        Apellidos = trabajador.Apellidos,
                        Email = trabajador.Email,
                        IDProvincia = trabajador.IDProvincia,
                        IDRol = 4
                    };
                    UsuarioController.CrearUsuario(ref usuario);
                   
                    if (usuario.ID > 0)
                    {
                        var trNB = db.Trabajador.Add(new Trabajador()
                        {
                            IDUsuario = usuario.ID,
                            NumSegSocial = trabajador.SS,
                            DNI = trabajador.DNI,
                            Localidad = trabajador.Localidad,
                            Nacionalidad = trabajador.Nacionalidad,
                            Telefono = trabajador.Telefono,                            
                            FechaAlta = DateTime.Now,
                            Activo = true
                        });

                        if (trabajador.Puestos != null)
                        {
                            foreach (int idPuesto in trabajador.Puestos)
                            {
                                db.PuestoTrabajador.Add(new PuestoTrabajador()
                                {
                                    Activo = true,
                                    IDCentro = idCentro,
                                    IDPuesto = idPuesto,
                                    IDUsuario = usuario.ID
                                });
                            }
                        }

                        db.CentroTrabajador.Add(new CentroTrabajador() { 
                            Activo = true,
                            IDCentro = idCentro,
                            IDTrabajador = trNB.IDUsuario
                        });
                        try
                        {
                            db.SaveChanges();
                            //SE ENVÍA EMAIL DE BIENVENIDA CON DATOS DE ACCECSO
                            Dictionary<string, string> datos = new Dictionary<string, string>();
                            datos.Add("##NOMBRECOMPLETO##", usuario.Nombre.ToUpper() + " " + usuario.Apellidos.ToUpper());
                            datos.Add("##EMAIL##", usuario.Email);
                            datos.Add("##CONTRASENYA##", usuario.Password);
                            Email.EnviarEmail("Bienvenido a la familia de NutriBuddy", "Content\\Templates\\NuevoTNB.html", datos);
                        }
                        catch (Exception e)
                        {
                            bandera = 1;
                            msg = "Error guardando los datos del trabajador";
                        }
                    }
                    else
                    {
                        bandera = 1;
                        msg = "Error guardando los datos del trabajador";
                    }
                }
                else
                {
                    var tC = (from t in db.Trabajador where t.Activo && t.IDUsuario == trabajador.ID select t).FirstOrDefault();
                    tC.IDUsuario = trabajador.ID;
                    tC.NumSegSocial = trabajador.SS;
                    tC.DNI = trabajador.DNI;
                    tC.Nacionalidad = trabajador.Nacionalidad;
                    tC.Localidad = trabajador.Localidad;
                    tC.Telefono = trabajador.Telefono;                        
                    db.Entry(tC).State = System.Data.Entity.EntityState.Modified;
                    var usu = (from u in db.Usuario where u.Activo && u.ID == trabajador.ID select u).FirstOrDefault();
                    usu.Nombre = trabajador.Nombre;
                    usu.Apellidos = trabajador.Apellidos;
                    usu.Email = trabajador.Email;
                    usu.IDProvincia = trabajador.IDProvincia;
                    db.Entry(usu).State = System.Data.Entity.EntityState.Modified;
                    var IDPuestosActuales = (from p in db.PuestoTrabajador where p.IDUsuario == usu.ID && p.Activo select p.IDPuesto).ToList();
                    foreach(int puesto in trabajador.Puestos)
                    {
                        if (IDPuestosActuales.Contains(puesto))
                        {
                            IDPuestosActuales.Remove(puesto);
                        }
                        else
                        {
                            var pAux = (from p in db.PuestoTrabajador where p.IDUsuario == usu.ID && p.IDPuesto == puesto && p.IDCentro == idCentro select p).FirstOrDefault();
                            if (pAux != null) {
                                pAux.Activo = true;
                                db.Entry(pAux).State = System.Data.Entity.EntityState.Modified;
                            }
                            else
                            {
                                db.PuestoTrabajador.Add(new PuestoTrabajador()
                                {
                                    IDUsuario = usu.ID,
                                    IDPuesto = puesto,
                                    IDCentro = idCentro,
                                    Activo = true
                                });
                            }
                        }
                    }

                    foreach(int puestoViejo in IDPuestosActuales)
                    {
                        var puesto = (from p in db.PuestoTrabajador where p.IDUsuario == usu.ID && p.Activo && p.IDCentro == idCentro 
                                      && p.IDPuesto == puestoViejo select p).FirstOrDefault();
                        puesto.Activo = false;
                        db.Entry(puesto).State = System.Data.Entity.EntityState.Modified;
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        bandera = 1;
                        msg = "Error guardando los datos del trabajador";
                    }

                }
                return Json(new { flag = bandera, msg = msg });
            }
        }
    }
}