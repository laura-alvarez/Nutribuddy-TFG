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
    public class PacienteController : ParentController
    {
        public ActionResult FormularioInicial()
        {
            ViewData["IDPaciente"] = Session["usuarioID"];
            ViewBag.Title = "NutriBuddy - " + Resources.forms.initialForm;
            return View("~/Views/Pacientes/Form_Inicial.cshtml");
        }
        
        public ActionResult SaveFormularioInicial(PacienteInicial dIniciales)
        {
            using(var db =new NutribuddyEntities())
            {
                var paciente = (from p in db.Paciente where p.IDUsuario == dIniciales.IdUsuario && p.Usuario.Activo select p).FirstOrDefault();
                if(paciente != null)
                {
                    paciente.IDObjetivo = dIniciales.IDObjetivo;
                    paciente.EsSedentario = dIniciales.EsSedentario;
                    paciente.TieneEstres = dIniciales.TieneEstres;
                    paciente.BajaAutoestima = dIniciales.BajaAutoestima;
                    paciente.FormularioInicial = true;

                    db.Entry(paciente).State = System.Data.Entity.EntityState.Modified;

                    db.PacienteMedidas.Add(new PacienteMedidas() {
                        Altura = ((int)dIniciales.Altura),
                        Peso = dIniciales.Peso,
                        CmPecho = dIniciales.Pecho,
                        CmCintura =  dIniciales.Cintura,
                        CmCadera = dIniciales.Cadera,
                        PGrasa = dIniciales.Grasa,
                        PMagro =  dIniciales.Musculo,
                        PAgua =  dIniciales.Agua,
                        Activo =  true,
                        PostedDate = DateTime.Now,
                        IDPaciente = paciente.IDUsuario
                    });


                    try
                    {
                        db.SaveChanges();
                    }catch(Exception e)
                    {

                    }
                }
            }
            return Json(new { });
        }

        public ActionResult GetPacientes()
        {
            int idCentro = int.Parse(Session["centroActual"].ToString());
            using(var db= new NutribuddyEntities())
            {
                var pacientes = (from p in db.Paciente
                                 where p.IDCentro == idCentro && p.Activo
                                 select new PacienteT()
                                 {
                                     ID = p.IDUsuario,
                                     Nombre = p.Usuario.Nombre+" "+p.Usuario.Apellidos,
                                     Email = p.Usuario.Email,
                                     Telefono = p.Telefono,
                                     ProxCita = (from c in db.Cita where c.IDPaciente == p.IDUsuario && c.Activo && c.HoraInicio >= DateTime.Now select c.HoraInicio).OrderBy(x=>x).FirstOrDefault()
                                 }).ToList();
                pacientes = pacientes.Select(p => new PacienteT() {
                    ID = p.ID,
                    Nombre = p.Nombre,
                    Email = p.Email,
                    Telefono = p.Telefono,
                    ProxCitaS = p.ProxCita != null && p.ProxCita.Year!=1?p.ProxCita.ToString("dd-MM-yyyy HH:mm"):"-"                    
                }).ToList();
                return Json(new { pacientes = pacientes }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GPaciente(int id)
        {
            using (var db = new NutribuddyEntities())
            {
                ViewData["idPaciente"] = id;
                Dictionary<string, List<ProvinciaBasica>> localizaciones = new Dictionary<string, List<ProvinciaBasica>>();
                var ccaa = (from ca in db.ComunidadAutonoma select ca).OrderBy(x => x.Nombre).ToList();
                foreach (ComunidadAutonoma ca in ccaa)
                {
                    var provincias = (from p in db.Provincia where p.IDCCAA == ca.ID select new ProvinciaBasica() { ID = p.ID, Nombre = p.Nombre }).OrderBy(x => x.Nombre).ToList();
                    localizaciones.Add(ca.Nombre, provincias);
                }
                ViewData["localizaciones"] = localizaciones;
                
                PacienteF paciente = (from p in db.Paciente where p.IDUsuario == id && p.Activo select new PacienteF() {
                    Nombre = p.Usuario.Nombre,
                    Apellidos = p.Usuario.Apellidos,
                    Email = p.Usuario.Email,
                    DNI = p.DNI,
                    Direccion = p.Direccion,
                    IDProvincia = p.Usuario.IDProvincia,
                    EsHombre = p.EsHombre,
                    Telefono = p.Telefono,
                    FNacimiento = p.FechaNacimiento
                }).FirstOrDefault();
                if (paciente == null) paciente = new PacienteF();
                ViewData["paciente"] = paciente;
                
                return View("/Views/Pacientes/GPaciente.cshtml");
            }
        }

        public ActionResult GuardarPaciente(PacienteF p)
        {
            int bandera = 0;
            string msg = p.ID == 0?"Paciente dado de alta correctamente, se le ha enviado un email con sus credenciales":"Información del paciente actualizada";
            int idCentro = int.Parse(Session["centroActual"].ToString());
            using (var db = new NutribuddyEntities())
            {
                if(p.ID == 0)
                {
                    var usuario = new Usuarios()
                    {
                        Nombre = p.Nombre,
                        Apellidos = p.Apellidos,
                        Email = p.Email,
                        IDRol = 5,
                        IDProvincia = p.IDProvincia                       
                    };
                    UsuarioController.CrearUsuario(ref usuario);
                    if(usuario.ID > 0)
                    {
                        var paciente = db.Paciente.Add(new Paciente()
                        {
                            IDUsuario = usuario.ID,
                            IDCentro = idCentro,
                            Activo = true,
                            DNI = p.DNI,
                            FechaNacimiento = p.FNacimiento,
                            Direccion = p.Direccion,
                            Telefono = p.Telefono,
                            EsHombre = p.EsHombre,
                            FormularioInicial = false,
                            BajaAutoestima = false,
                            TieneEstres = false,
                            EsSedentario = false
                        });

                        try
                        {
                            db.SaveChanges();
                            //SE ENVÍA EMAIL DE BIENVENIDA CON DATOS DE ACCECSO
                            var NombreCentro = (from nc in db.Centro where nc.ID == paciente.IDCentro select nc.NombreCentro).FirstOrDefault();
                            Dictionary<string, string> datos = new Dictionary<string, string>();
                            datos.Add("##NOMBRECOMPLETO##", usuario.Nombre.ToUpper() + " " + usuario.Apellidos.ToUpper());
                            datos.Add("##NOMBRECENTRO##", NombreCentro);
                            datos.Add("##CONTRASENYA##", usuario.Password);
                            Email.EnviarEmail("Bienvenido a "+ NombreCentro, "Content\\Templates\\NuevoPacientePZ.html", datos);
                        }
                        catch(Exception e)
                        {
                            bandera = 1;
                            msg = "¡Ups! Ha ocurrido un error al dar de alta el paciente";
                        }
                    }
                }
                else
                {
                    var paciente = (from pct in db.Paciente where pct.IDUsuario == p.ID && pct.IDCentro == idCentro select pct).FirstOrDefault();
                    if(paciente != null)
                    {
                            paciente.DNI = p.DNI;
                            paciente.FechaNacimiento = p.FNacimiento;
                            paciente.Direccion = p.Direccion;
                            paciente.Telefono = p.Telefono;
                            paciente.EsHombre = p.EsHombre;
                            db.Entry(paciente).State = System.Data.Entity.EntityState.Modified;

                        var usuario = (from u in db.Usuario where u.ID == p.ID select u).FirstOrDefault();
                        if(usuario != null)
                        {
                            usuario.Nombre = p.Nombre;
                            usuario.Apellidos = p.Apellidos;
                            usuario.Email = p.Email;
                            usuario.IDProvincia = p.IDProvincia;
                            db.Entry(usuario).State = System.Data.Entity.EntityState.Modified;
                        }
                        try
                        {
                            db.SaveChanges();
                        }catch(Exception e)
                        {
                            bandera = 1;
                            msg = "¡Ups! Ha ocurrido un error al actulizar el paciente";
                        }
                           
                    }
                }
                return Json(new { flag = bandera, msg = msg });
            }
        }

        public ActionResult EliminarPaciente(int id)
        {
            int bandera = 0;
            string msg = "Paciente dado de baja";
            Usuario usuario = Session["usuario"] as Usuario;
            using (var db = new NutribuddyEntities())
            {
                var paciente = (from p in db.Paciente
                                    where p.Activo && p.IDUsuario == id
                                    select p).FirstOrDefault();
                if (paciente != null)
                {
                    paciente.Activo = false;
                    paciente.Usuario.Activo = false;
                    paciente.Usuario.FechaBaja = DateTime.Now;
                    db.Entry(paciente).State = System.Data.Entity.EntityState.Modified;

                    var citas = (from c in db.Cita where c.IDPaciente == paciente.IDUsuario && c.Activo && c.HoraInicio >= DateTime.Now select c).ToList();
                    foreach(Cita c in citas)
                    {
                        c.Activo = false;
                        db.Entry(c).State = System.Data.Entity.EntityState.Modified;
                    }

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
            return Json(new { flag = bandera, msg = msg });
        }

        public ActionResult GetCitasPaciente()
        {
            Usuario usuario = Session["usuario"] as Usuario;
            using (var db = new NutribuddyEntities())
            {
                var citasBD = (from c in db.Cita 
                             where c.IDPaciente == usuario.ID
                             select c).ToList();
                var citas = citasBD.Select(c => new EventoCita()
                {
                    id = c.ID.ToString(),
                    title = c.Titulo,
                    start = c.HoraInicio.ToString("s"),
                    end = c.HoraFin.ToString("s"),
                    description = c.Observaciones,
                    backgroundColor = c.HoraInicio>DateTime.Now?"#fe9f3c": "#9fdc64",
                    borderColor = c.HoraInicio > DateTime.Now ? "#fe9f3c" : "#9fdc64",
                    textColor = "black"
                }).ToList();

                return Json(new {citas = citas }, JsonRequestBehavior.AllowGet);
            }
        }

        public class CitaCompletada
        {
            public int ID { get; set; }
            public string Dia { get; set; }
            public string Tipo { get; set; }
            public string Trabajador { get; set; }
            public string Pautas { get; set; }
        }

       

        public ActionResult GetDetalleCita(int idCita)
        {
            using(var db=new NutribuddyEntities())
            {
                var citaDB = (from c in db.Cita where c.ID == idCita select c).FirstOrDefault();
                if(citaDB != null)
                {                    
                    var cita = new CitaCompletada() {
                        ID = citaDB.ID,
                        Dia = citaDB.HoraInicio.ToString("dd-MM-yyyy hh:mm"),
                        Tipo = citaDB.Puesto.Nombre,
                        Trabajador =citaDB.Trabajador.Usuario.Nombre + " "+ citaDB.Trabajador.Usuario.Apellidos,
                        Pautas = citaDB.PautasPaciente
                    };
                    return Json(new {flag= 0, completada = citaDB.Completada, cita = cita }, JsonRequestBehavior.AllowGet);                    
                }
                return Json(new { flag = 1}, JsonRequestBehavior.AllowGet);
            }
        }

    }
}