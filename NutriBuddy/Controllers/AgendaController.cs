using NutriBuddy.Models;
using NutriBuddy.Models.Propios;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NutriBuddy.Controllers
{
    public class AgendaController : ParentController
    {
        // GET: Agenda
        public ActionResult Agenda()
        {
            return View("~/Centros/Agenda.cshtml");
        }

        public ActionResult GetCalendarioCitas(int idTrabajador)
        {
            using (var db = new NutribuddyEntities())
            {
                int IDCentro = int.Parse(Session["centroActual"].ToString());
                var citasBD = (from c in db.Cita
                             where c.Activo && c.IDCentro == IDCentro
                               select c).ToList();
                var citas = citasBD.Select(c => new EventoCita()
                {
                    id = c.ID.ToString(),
                    title = c.Titulo,
                    start = c.HoraInicio.ToString("s"),
                    end = c.HoraFin.ToString("s"),
                    description = c.Observaciones,
                    backgroundColor = c.Puesto.colorAgenda,
                    borderColor = c.Puesto.colorAgenda,
                    textColor = "black"

                }).ToList();
                return Json(new { events = citas });
            }           
        }

        public ActionResult GetDetalleCita(int idCita)
        {
            int error = 0;
            using (var db = new NutribuddyEntities())
            {
                Usuario usuario = Session["usuario"] as Usuario;
                var puedeAdministrar = (from pt in db.PuestoTrabajador where pt.IDPuesto == 1 && pt.IDUsuario == usuario.ID && pt.Activo select pt).Any();
               
                var cita = (from c in db.Cita
                            where c.Activo && c.ID == idCita
                            select c).FirstOrDefault();
                if (cita != null)
                {
                    var citaFront = new EventoCitaDetalles()
                    {
                        ID = cita.ID,
                        IDTipo = cita.IDTipo,
                        IDTrabajador = cita.IDTrabajador,
                        IDPaciente = cita.IDPaciente,
                        Dia = cita.HoraInicio.ToString("yyyy-MM-dd"),
                        HoraInicio = cita.HoraInicio.ToString("HH:mm"),                        
                        HoraFin = cita.HoraFin.ToString("HH:mm"),
                        Titulo = cita.Titulo,
                        Observaciones = cita.Observaciones
                    };

                    var PuedeComenzar = (from c in db.Cita where c.ID == idCita && !c.Completada && c.IDTrabajador == usuario.ID select c).Any();
                    return Json(new { cita = citaFront, flag = error, PuedeComenzar = PuedeComenzar, PuedeAdministrar = puedeAdministrar }, JsonRequestBehavior.AllowGet); 
                }
                return Json(new { flag = error, msg = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GuardarCita(EventoCitaGuardar evento)
        {
            int error = 0;
            string msg = evento.ID == 0?"Cita añadida":"Cita editada correctamente";

            int IDCentro = int.Parse(Session["centroActual"].ToString());
            using (var db = new NutribuddyEntities())
            {
                DateTime dia = new DateTime(evento.Dia.Year, evento.Dia.Month, evento.Dia.Day);
                DateTime inicio = dia + evento.HoraInicio;
                DateTime fin = dia + evento.HoraFin;

                if (evento.ID == 0) {
                    db.Cita.Add(new Cita()
                    {
                        Activo = true,
                        FechaAlta = DateTime.Now,
                        IDTipo = evento.IDTipo,
                        IDCentro= IDCentro,
                        IDTrabajador = evento.IDTrabajador,
                        IDPaciente = evento.IDPaciente,
                        Titulo = evento.Titulo,
                        Observaciones = evento.Observaciones,
                        HoraInicio = inicio,
                        HoraFin = fin
                    });
                }
                else
                {
                    var cita = db.Cita.Find(evento.ID);
                    if(cita != null)
                    {
                        cita.Titulo = evento.Titulo;
                        cita.Observaciones = evento.Observaciones;
                        cita.HoraInicio = inicio;
                        cita.HoraFin = fin;
                        db.Entry(cita).State = System.Data.Entity.EntityState.Modified;
                    }
                }

                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    error = 1;
                    msg = "Error al intentar agendar la cita, intentelo más tarde";
                }
            }

            return Json(new { flag = error, msg = msg });
        }

        [HttpDelete]
        public ActionResult EliminarCita(int idCita)
        {
            int error = 0;
            string msg = "Cita eliminada";
            using(var db=new NutribuddyEntities())
            {
                var cita = (from c in db.Cita where c.ID == idCita && c.Activo select c).FirstOrDefault();
                if (cita != null)
                {
                    cita.Activo = false;
                    db.Entry(cita).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    error = 1;
                    msg = "Error al eliminar la cita";
                }
                return Json(new { flag = error, msg = msg });
            }
        }

        public ActionResult GetTrabajadoresPuesto (int idPuesto)
        {
            using(var db= new NutribuddyEntities())
            {
                int IDCentro = int.Parse(Session["centroActual"].ToString());
                var trabajadoresCentro = (from p in db.PuestoTrabajador 
                                         
                                    where p.Activo && p.IDCentro == IDCentro && p.IDPuesto == idPuesto
                                    select new Basico() { 
                                    ID = p.IDUsuario,
                                    Nombre = p.Usuario.Nombre+" "+ p.Usuario.Apellidos
                                    }).ToList();

                return Json(new {trabajadores = trabajadoresCentro }, JsonRequestBehavior.AllowGet);
            }
        } 
        public ActionResult GetPacientesTrabajador(int idTrabajador)
        {
            using(var db= new NutribuddyEntities())
            {
                int IDCentro = int.Parse(Session["centroActual"].ToString());
                var pacientesTrabajador = (from p in db.Paciente
                                    where p.Usuario.Activo && p.IDCentro == IDCentro                                    
                                    select new Basico() { 
                                    ID = p.IDUsuario,
                                    Nombre = p.Usuario.Nombre+" "+ p.Usuario.Apellidos
                                    }).ToList();

                return Json(new {pacientes = pacientesTrabajador }, JsonRequestBehavior.AllowGet);
            }
        }

        #region Citas
        public ActionResult ComenzarCita(int idCita)
        {
            int IDCentro = int.Parse(Session["centroActual"].ToString());
            using (var db = new NutribuddyEntities())
            {
                var cita = (from c in db.Cita where c.ID == idCita select c).FirstOrDefault();
                var citaAnterior = (from c in db.Cita where c.HoraFin <= cita.HoraInicio && c.IDTipo == cita.IDTipo && c.IDPaciente == cita.IDPaciente select c).OrderByDescending(x => x.HoraFin).FirstOrDefault();
                ViewData["IDCita"] = cita.ID;
                ViewData["nombrePaciente"] = cita.Paciente.Usuario.Nombre + " " + cita.Paciente.Usuario.Apellidos;
                ViewData["fechaCita"] = cita.HoraInicio.ToString("dd-MM-yyyy HH:mm")+"-"+cita.HoraFin.ToString("HH:mm");
                ViewData["hayPrevia"] = citaAnterior != null;
                
                ViewData["previaFecha"] = citaAnterior != null?citaAnterior.HoraInicio.ToString("dd-MM-yyyy"): "";
                ViewData["previaTrabajador"] = citaAnterior != null?citaAnterior.Trabajador.Usuario.Nombre+" "+ citaAnterior.Trabajador.Usuario.Apellidos: "";
                ViewData["previaNotasPrivadas"] = citaAnterior != null && citaAnterior.NotasPrivadas != null? citaAnterior.NotasPrivadas : "";
                ViewData["previaPautas"] = citaAnterior != null && citaAnterior.PautasPaciente != null? citaAnterior.PautasPaciente : "";

                return View("~/Views/Agenda/Cita.cshtml");
            }
        }

        public class DatosCita
        {
            public int ID { get; set; }
            public string Notas { get; set; }
            public string Pautas { get; set; }
        }
        public ActionResult CompletarCita(DatosCita cita)
        {
            int bandera = 0;
            string msg = "Cita completada";
            using(var db=  new NutribuddyEntities())
            {
                var citaBD = (from c in db.Cita where c.ID == cita.ID select c).FirstOrDefault();
                if(citaBD != null)
                {
                    citaBD.Completada = true;
                    citaBD.NotasPrivadas = cita.Notas;
                    citaBD.PautasPaciente = cita.Pautas;
                    db.Entry(citaBD).State = System.Data.Entity.EntityState.Modified;

                    try
                    {
                        db.SaveChanges();
                    }catch(Exception e)
                    {
                        bandera = 1;
                        msg = "Ha ocurrido un error al guardar la cita";
                    }
                }
            }
            return Json(new {flag = bandera, msg = msg });
        }
        #endregion
    }


}