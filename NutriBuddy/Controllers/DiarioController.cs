using NutriBuddy.Models;
using NutriBuddy.Models.Propios;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NutriBuddy.Controllers
{
    public class DiarioController : ParentController
    {
        // GET: Agenda
        public ActionResult Diario()
        {
            using(
                var db=  new NutribuddyEntities())
            {
                var emociones = (from e in db.EstadoAnimicoTipo
                                 select new Basico()
                                 {
                                     ID = e.ID,
                                     Nombre = e.Nombre,
                                     NombreEN = e.NombreEN
                                 }).ToList();
                ViewData["emociones"] = emociones;
            }
            return View("~/Diario/Diario.cshtml");
        }

        public ActionResult GetEntradasDiario()
        {
            using (var db = new NutribuddyEntities())
            {
                var usuario = Session["usuario"] as Usuario;
                var diario = (from c in db.Diario
                             where c.Activo && usuario.ID == c.IDPaciente
                             select c).ToList();
                var entrada = diario.Select(c => new EntradaDiario()
                {
                    allDay = true,
                    id = c.ID.ToString(),
                    title = Resources.diario.diario_calendario,
                    start = c.Dia.ToString("yyyy-MM-dd 00:00:00"),
                    end = c.Dia.ToString("yyyy-MM-dd 00:00:00"),
                    backgroundColor = "#ffbd00",
                    borderColor = "#ffbd00",
                    textColor = "black",
                    display = "background"

                }).ToList();
                
                return Json(new { events = entrada });
            }           
        }

        public ActionResult GuardarDiario(Models.Propios.Diario diarioG)
        {
            int error = 0;
            string msg = "Diario guardado";
            bool guardadaPrimeraFase = false;
            using (var db = new NutribuddyEntities())
            {
                Usuario usuario = Session["usuario"] as Usuario;
                var diaS = diarioG.Dia.Split('-');
                var dia = new DateTime(int.Parse(diaS[2]), int.Parse(diaS[1]), int.Parse(diaS[0]));
                //Se busca si existe un diario en ese día para el usuario logueado
                var diario = (from d in db.Diario where d.Activo && d.IDPaciente == usuario.ID && d.Dia == dia select d).FirstOrDefault(); 
                if(diario == null)
                {
                    var medidas = db.PacienteMedidas.Add(new PacienteMedidas() { 
                        Activo = true,
                        PostedDate = dia,
                        IDPaciente = usuario.ID,
                        Altura = diarioG.Medidas.Altura,
                        Peso = diarioG.Medidas.Peso,
                        CmPecho = diarioG.Medidas.Pecho,
                        CmCintura = diarioG.Medidas.Cintura,
                        CmCadera = diarioG.Medidas.Cadera,
                        PAgua = diarioG.Medidas.Agua,
                        PGrasa = diarioG.Medidas.Grasa,
                        PMagro = diarioG.Medidas.Musculo
                    });
                    //NO EXISTE SE AÑADE UNA NUEVA ENTRADA
                    diario = db.Diario.Add(new Models.Diario()
                    {
                        Dia = dia,
                        IDPaciente = usuario.ID,
                        IDMedidas = medidas.ID,
                        AguaCantidad = diarioG.Agua,
                        Medicamentos = diarioG.Medicamentos,
                        Menstruaccion = diarioG.Menstruaccion,
                        Activo = true,
                        IDSuenyo = diarioG.IDSuenyo,
                        IDEstado = diarioG.IDEstado,
                        Deporte = diarioG.Deporte,
                        ObsDeporte = diarioG.DeporteObservaciones
                    });
                    


                    try
                    {
                        db.SaveChanges();
                        guardadaPrimeraFase = true;
                    }
                    catch(Exception e)
                    {
                        error = 1;
                        msg = "Error al guardar el diario, intentelo de nuevo más tarde";
                    }
                }
                else
                {
                    PacienteMedidas medidas;
                    //EXISTE, SE EDITA
                    if(diario.IDMedidas != null)
                    {
                        medidas = (from m in db.PacienteMedidas where m.ID == diario.IDMedidas select m).FirstOrDefault();

                        medidas.Altura = diarioG.Medidas.Altura;
                        medidas.Peso = diarioG.Medidas.Peso;
                        medidas.CmPecho = diarioG.Medidas.Pecho;
                        medidas.CmCintura = diarioG.Medidas.Cintura;
                        medidas.CmCadera = diarioG.Medidas.Cadera;
                        medidas.PAgua = diarioG.Medidas.Agua;
                        medidas.PGrasa = diarioG.Medidas.Grasa;
                        medidas.PMagro = diarioG.Medidas.Musculo;
                        db.Entry(medidas).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        medidas = db.PacienteMedidas.Add(new PacienteMedidas()
                        {
                            Activo = true,
                            PostedDate = dia,
                            IDPaciente = usuario.ID,
                            Altura = diarioG.Medidas.Altura,
                            Peso = diarioG.Medidas.Peso,
                            CmPecho = diarioG.Medidas.Pecho,
                            CmCintura = diarioG.Medidas.Cintura,
                            CmCadera = diarioG.Medidas.Cadera,
                            PAgua = diarioG.Medidas.Agua,
                            PGrasa = diarioG.Medidas.Grasa,
                            PMagro = diarioG.Medidas.Musculo
                        });
                        diario.IDMedidas = medidas.ID;
                    }

                    diario.IDPaciente = usuario.ID;
                    diario.AguaCantidad = diarioG.Agua;
                    diario.Medicamentos = diarioG.Medicamentos;
                    diario.Menstruaccion = diarioG.Menstruaccion;
                    diario.IDSuenyo = diarioG.IDSuenyo;
                    diario.IDEstado = diarioG.IDEstado;
                    diario.Deporte = diarioG.Deporte;
                    diario.ObsDeporte = diarioG.DeporteObservaciones;                    
                    db.Entry(diario).State = System.Data.Entity.EntityState.Modified;


                    var comidasViejas = (from c in db.Comida where c.Activo && c.IDDiario == diario.ID select c).ToList();
                    foreach(Models.Comida c in comidasViejas)
                    {
                        c.Activo = false;
                        db.Entry(c).State = System.Data.Entity.EntityState.Modified;
                    }
                    var emocionesViejas = (from e in db.EstadoAnimicoDiario where e.Activo && e.IDDiario == diario.ID select e).ToList();
                    foreach(EstadoAnimicoDiario e in emocionesViejas)
                    {
                        e.Activo = false;
                        db.Entry(e).State = System.Data.Entity.EntityState.Modified;
                    }
                    var deporteViejo = (from d in db.Deporte where d.Activo && d.IDDiario == diario.ID select d).ToList();
                    foreach(Deporte d in deporteViejo)
                    {
                        d.Activo = false;
                        db.Entry(d).State = System.Data.Entity.EntityState.Modified;
                    }
                    try
                    {
                        db.SaveChanges();
                        guardadaPrimeraFase = true;
                    }
                    catch(Exception e)
                    {
                        error = 1;
                        msg = "Error al guardar el diario, intentelo de nuevo más tarde";
                    }
                }

                if (guardadaPrimeraFase)
                {
                    //SE AGREGAN LOS ELEMENTOS COMUNES
                    if (diarioG.Comidas != null)
                    {
                        foreach (Models.Propios.Comida com in diarioG.Comidas)
                        {
                            DateTime horaComida = dia;
                            if(com.Hora != null)
                            {
                                horaComida = horaComida.AddHours(com.Hora.Hours).AddMinutes(com.Hora.Minutes);
                            }
                            db.Comida.Add(new Models.Comida()
                            {
                                IDDiario = diario.ID,
                                IDTipoComida = com.IDTipo,
                                Alimentos = com.Alimentos,
                                Dia = horaComida,
                                Observaciones = com.Observaciones == null ? "" : com.Observaciones,
                                Activo = true
                            });
                        }
                    }

                    if (diarioG.Emociones != null)
                    {
                        foreach (int idEmocion in diarioG.Emociones)
                        {
                            db.EstadoAnimicoDiario.Add(new EstadoAnimicoDiario()
                            {
                                IDDiario = diario.ID,
                                IDEstadoAnimico = idEmocion,
                                Activo = true
                            });
                        }
                    }
                    if (diarioG.Deporte && diarioG.Deportes != null)
                    {
                        foreach (int deporte in diarioG.Deportes)
                        {
                            db.Deporte.Add(new Models.Deporte()
                            {
                                Activo = true,
                                IDDiario = diario.ID,
                                IDTipoDeporte = deporte
                            });
                        }
                    }
                    try
                    {
                        db.SaveChanges();
                    }catch(Exception e)
                    {
                        error = 2;
                        msg = "Error al guardar el diario, intentelo de nuevo más tarde";

                    }
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

        public ActionResult TieneDiario(DateTime dia)
        {
            DateTime dia00 = new DateTime(dia.Year, dia.Month, dia.Day, 0, 0, 0);
            var usuario = Session["usuario"] as Usuario;
            Models.Propios.Diario entradaDiario = null;
            if (usuario != null)
            {
                using (var db = new NutribuddyEntities()) {
                    entradaDiario = (from d in db.Diario 
                                         where d.IDPaciente == usuario.ID && d.Dia == dia00 && d.Activo 
                                         select new Models.Propios.Diario() { 
                                            Dia = d.Dia.ToString(),
                                            Comidas = (from c in db.Comida where c.IDDiario == d.ID && c.Activo select new Models.Propios.Comida()
                                            {
                                                ID = c.ID,
                                                IDTipo = c.IDTipoComida, 
                                                Dia = c.Dia,
                                                Alimentos = c.Alimentos,
                                                Observaciones = c.Observaciones
                                            }).ToList(),
                                            Medidas = (from m in db.PacienteMedidas where m.ID == d.IDMedidas select new Models.Propios.Medidas()
                                            {
                                                Altura = m.Altura == null?0:(decimal)m.Altura,
                                                Peso = m.Peso==null?0:(decimal)m.Peso,
                                                Pecho = m.CmPecho == null?0:(decimal)m.CmPecho,
                                                Cintura = m.CmCintura==null?0:(decimal)m.CmCintura,
                                                Cadera = m.CmCadera==null?0:(decimal)m.CmCadera,
                                                Agua = m.PAgua==null?0:(decimal)m.PAgua,
                                                Grasa = m.PGrasa == null ? 0 : (decimal)m.PGrasa,
                                                Musculo = m.PMagro == null ? 0 : (decimal)m.PMagro
                                            }).FirstOrDefault(),
                                            IDEstado = d.IDEstado == null?1:(int)d.IDEstado,
                                            Emociones = (from e in db.EstadoAnimicoDiario where e.IDDiario == d.ID && e.Activo select e.IDEstadoAnimico).ToList(),
                                            Agua = d.AguaCantidad == null?0:(decimal)d.AguaCantidad,
                                            IDSuenyo = d.IDSuenyo == null?1:(int)d.IDSuenyo,
                                            Medicamentos = d.Medicamentos,
                                            Menstruaccion = d.Menstruaccion == null?false:(bool)d.Menstruaccion,
                                            Deporte = d.Deporte,
                                            Deportes = (from de in db.Deporte where de.IDDiario == d.ID && de.Activo select de.IDTipoDeporte).ToList(),
                                            DeporteObservaciones = d.ObsDeporte
                                         }).FirstOrDefault();
                        
                }

                if (entradaDiario != null && entradaDiario.Comidas != null)
                {
                    List<Models.Propios.Comida> comidas = new List<Models.Propios.Comida>();
                    foreach (Models.Propios.Comida c in entradaDiario.Comidas)
                    {
                        c.HoraS = c.Dia.ToString("HH:mm");
                        comidas.Add(c);
                    }
                    entradaDiario.Comidas = comidas;
                }
            }
            return Json(new { existe = entradaDiario != null, diario = entradaDiario }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CalcularIMC(double altura, double peso)
        {
            int imc = 0;
            if (altura != 0)
            {
                imc = (int)Math.Floor(peso / Math.Pow(altura / 100, 2));
            }
            return Json(new {imc = imc }, JsonRequestBehavior.AllowGet);
        }
    }

   
}