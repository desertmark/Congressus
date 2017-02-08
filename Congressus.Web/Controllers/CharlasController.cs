using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Congressus.Web.Context;
using Congressus.Web.Models.Entities;
using Congressus.Web.Repositories;
using Congressus.Web.Models;

namespace Congressus.Web.Controllers
{
    //[Authorize(Roles = "admin")]
    public class CharlasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private CharlasRepository _repo = new CharlasRepository();

        // GET: Charlas
        [Authorize(Roles = "admin,presidente")]
        public ActionResult Index()
        {
            return View(_repo.GetAll().OrderBy(c =>c.Evento));
        }

        // GET: Charlas/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            Charla charla = _repo.FindById(id);
            if (charla == null)
            {
                return HttpNotFound();
            }
            return View(charla);
        }

        // GET: Charlas/Create
        [Authorize(Roles = "admin,presidente")]
        public ActionResult Create(int eventoId)
        {
            var vm = _repo.GetCharlaViewModel(eventoId);            
            return View(vm);
        }

        // POST: Charlas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="admin,presidente")]
        public ActionResult Create(CharlaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var charla = _repo.GetCharlaFromVm(model);
                if(charla.FechaHora > charla.Evento.FechaInicio && charla.FechaHora < charla.Evento.FechaFin) { 
                    _repo.Add(charla);
                    return RedirectToAction("Administrar","Eventos", new { id = model.EventoId});
                }else
                {
                    ModelState.AddModelError("Fecha", "La fecha y hora especificada debe ser posterior a la fecha y hora de inicio del evento y anterior a su fecha de fin");
                }
            }
            model.Papers = _repo.GetCharlaViewModel(model.EventoId).Papers;
            return View(model);
        }

        // GET: Charlas/Edit/5
        [Authorize(Roles = "admin,presidente")]
        public ActionResult Edit(int id)
        {
            var charla = _repo.FindById(id);
            var model = _repo.GetCharlaViewModel(charla);
            if (charla == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: Charlas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CharlaViewModel model)
        {

            if (ModelState.IsValid)
            {
                var charla = _repo.GetCharlaFromVm(model);
                if (charla.FechaHora > charla.Evento.FechaInicio && charla.FechaHora < charla.Evento.FechaFin)
                {
                    _repo.Add(charla);
                    return RedirectToAction("Administrar", "Eventos", new { id = model.EventoId });
                }
                else
                {
                    ModelState.AddModelError("Fecha", "La fecha y hora especificada debe ser posterior a la fecha y hora de inicio del evento y anterior a su fecha de fin");
                }
                _repo.Edit(charla);
                return RedirectToAction("Administrar","Eventos",new { id = charla.Evento.Id });
            }
            return View(model);
        }

        // GET: Charlas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Charla charla = db.Charlas.Find(id);
            if (charla == null)
            {
                return HttpNotFound();
            }
            return View(charla);
        }

        // POST: Charlas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Charla charla = _repo.FindById(id);
            if(charla == null)
            {
                return HttpNotFound();
            }
            var eventoId = charla.Evento.Id;
            _repo.Delete(id);
            return RedirectToAction("Administrar", "Eventos", new { id = eventoId});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
