using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Congressus.Web.Context;
using Congressus.Web.Models.Entities;
using Congressus.Web.Repositories;
using Congressus.Web.Models;

namespace Congressus.Web.Controllers
{
    [Authorize]
    public class AsistentesController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        private AsistentesRepository _repo = new AsistentesRepository();
        [Authorize(Roles = "admin")]
        // GET: Asistentes
        public ActionResult Index()
        {
            var asistentes = _repo.GetAll();
            return View(asistentes.ToList());
        }
        [Authorize(Roles = "admin,asistente")]
        // GET: Asistentes/Details/5
        public ActionResult Details(int? id)
        {
            Asistente asistente;
            if (id == null)
            {
                var uid = User.Identity.GetUserId();
                asistente = _repo.FindByUserId(uid);
                return View(asistente);
            }
            asistente = _repo.FindById((int)id);
            if (asistente == null)
            {
                return HttpNotFound();
            }
            return View(asistente);
        }

        // GET: Asistentes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Asistentes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="admin")]
        public ActionResult Create(Asistente asistente)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(asistente);
                return RedirectToAction("Index");
            }
            return View(asistente);
        }
        [Authorize(Roles = "admin, asistente")]
        // GET: Asistentes/Edit/5
        public ActionResult Edit(int id)
        {
            Asistente asistente = _repo.FindById(id);
            if (asistente == null)
            {
                return HttpNotFound();
            }
            return View(asistente);
        }

        // POST: Asistentes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, asistente")]
        public ActionResult Edit(Asistente asistente)
        {
            if (ModelState.IsValid)
            {
                _repo.Edit(asistente);
                if (User.IsInRole("asistente")) {
                    return RedirectToAction("Details");
                }
                else{
                    return RedirectToAction("Details", new { id = asistente.Id });
                }
            }
            return View(asistente);
        }

        // GET: Asistentes/Delete/5
        public ActionResult Delete(int id)
        {
            Asistente asistente = _repo.FindById(id);
            if (asistente == null)
            {
                return HttpNotFound();
            }
            return View(asistente);
        }

        // POST: Asistentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _repo.Delete(id);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin, asistente")]
        public ActionResult InscribirEvento(InscripcionEventoViewModel model)
        {
            Asistente asistente = null;
            if (User.IsInRole("admin"))
                asistente = _repo.FindById(model.AsistenteId);
            else
                asistente = _repo.FindByUserId(User.Identity.GetUserId());        
            var evento = _repo.FindEventoById(model.EventoId);
            if (asistente == null || evento == null)
            {
                ViewBag.Mensaje = "El asistente, el evento o ambos son inexistentes.";
                return View("Error");
            }
            _repo.InscribirEvento(asistente, evento);
            return RedirectToAction("Details", "Asistentes");
        }

        [Authorize(Roles="admin,asistente")]
        public ActionResult InscribirCharla(InscripcionCharlaViewModel model)
        {
            Asistente asistente = null;
            if (User.IsInRole("admin"))
                asistente = _repo.FindById(model.AsistenteId);
            else
                asistente = _repo.FindByUserId(User.Identity.GetUserId());
            var charla = _repo.FindCharlaById(model.CharlaId);
            if (asistente == null || charla == null)
            {
                ViewBag.Mensaje = "El asistente, el evento o ambos son inexistentes.";
                return View("Error");
            }

            bool inscriptoEvento = charla.Evento.Inscripciones.Any(x => x.Asistente.UsuarioId == User.Identity.GetUserId());
            bool inscriptoCharla = charla.Inscripciones.Any(x => x.Asistente.UsuarioId == User.Identity.GetUserId());
            bool hayCupo = charla.Cupo > charla.Inscripciones.Count;
            bool charlaSinCupo = charla.Cupo == 0;

            if (!charlaSinCupo && !hayCupo)
            {
                ViewBag.Mensaje = "No hay mas cupo para esta charla.";
                return View("Error");
            }
            else if (inscriptoCharla)
            {
               ViewBag.Mensaje = "Ya estas inscripto a esta charla.";
               return View("Error");
            }
            else if (!inscriptoEvento)
            {
                ViewBag.Mensaje = "Debes inscribirte al evento primero.";
                return View("Error");
            }
            _repo.InscribirCharla(asistente, charla);
            return RedirectToAction("Details", "Asistentes");
        }

        [Authorize(Roles ="admin, asistente")]
        public ActionResult EliminarInscripcionEvento(int inscripcionId)
        {
            var inscripcion = _repo.BuscarInscripcionEvento(inscripcionId);
            if(inscripcion == null)
            {
                ViewBag.Codigo = 404;
                ViewBag.Mensaje = "Inscripcion no encontrada. Probablemente Ud no este inscripto a este evento.";
                return View("Error");
            }
            if (!User.IsInRole("admin") && inscripcion.Asistente.UsuarioId != User.Identity.GetUserId())
            {
                ViewBag.Codigo = 403;
                ViewBag.Mensaje = "Usted no tiene autorizacion para eliminar esta inscripcion.";
                return View("Error");
            }
            _repo.EliminarInscripcionEvento(inscripcion);
            return RedirectToAction("Details");
        }

        public ActionResult EliminarInscripcionCharla(int inscripcionId)
        {
            var inscripcion = _repo.BuscarInscripcionCharla(inscripcionId);
            if (inscripcion == null)
            {
                ViewBag.Codigo = 404;
                ViewBag.Mensaje = "Inscripcion no encontrada. Probablemente Ud no este inscripto a esta charla.";
                return View("Error");
            }
            if (!User.IsInRole("admin") && inscripcion.Asistente.UsuarioId != User.Identity.GetUserId())
            {
                ViewBag.Codigo = 403;
                ViewBag.Mensaje = "Usted no tiene autorizacion para eliminar esta inscripcion.";
                return View("Error");
            }
            _repo.EliminarInscripcionCharla(inscripcion);
            return RedirectToAction("Details");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
