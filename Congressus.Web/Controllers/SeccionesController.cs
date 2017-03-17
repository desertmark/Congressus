using Microsoft.AspNet.Identity;
using Congressus.Web.Models.Entities;
using Congressus.Web.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Congressus.Web.Models;

namespace Congressus.Web.Controllers
{
    [Authorize(Roles ="admin,presidente")]
    public class SeccionesController : Controller
    {
        private readonly SeccionesRepository _repo = new SeccionesRepository();
        // GET: Secciones/
        public ActionResult Index(int id)
        {
            var evento = _repo.FindEventoById(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            if (!ValidarPresidente(evento))
            {
                return new HttpUnauthorizedResult();
            }
            return View(evento);
        }
        // GET: Secciones/Details/5
        public ActionResult Details(int id)
        {
            var seccion = _repo.FindById(id);
            if (seccion == null)
            {
                return HttpNotFound();
            }
            if (!ValidarPresidente(seccion.Evento))
            {
                return new HttpUnauthorizedResult();
            }            
            return View(seccion);
        }

        // GET: Secciones/Create
        public ActionResult Create(int id)
        {
            var evento = _repo.FindEventoById(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            if (!ValidarPresidente(evento))
            {
                return new HttpUnauthorizedResult();
            }
            var model = new SeccionEventoVM() { EventoId = evento.Id };
            return View(model);
        }

        // POST: Secciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SeccionEventoVM model)
        {
            if (ModelState.IsValid)
            {
                var evento = _repo.FindEventoById(model.EventoId);
                if(evento==null)
                    return HttpNotFound();
                if (!ValidarPresidente(evento))
                    return new HttpUnauthorizedResult();

                var seccion = _repo.GetSeccionFromVM(model, evento);
                _repo.Add(seccion);
                return RedirectToAction("Index", new { id = model.EventoId });
            }
            return View(model);
        }

        // GET: Secciones/Edit/5
        public ActionResult Edit(int id)
        {
            var seccion = _repo.FindById(id);
            if (seccion == null)
                return HttpNotFound();

            if (!ValidarPresidente(seccion.Evento))
                return new HttpUnauthorizedResult();

            return View(_repo.GetVMFromSeccion(seccion));
        }

        // POST: Secciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SeccionEventoVM model)
        {
            var seccion = _repo.GetSeccionFromVM(model);
            if (seccion == null)
                return HttpNotFound();
            if (!ValidarPresidente(seccion.Evento))
                return new HttpUnauthorizedResult();
            _repo.Edit(seccion);
            return RedirectToAction("index", new { id = model.EventoId });
        }

        // GET: Secciones/Delete/5
        public ActionResult Delete(int id)
        {
            var seccion = _repo.FindById(id);
            if (seccion == null)
                return HttpNotFound();
            if (!ValidarPresidente(seccion.Evento))
                return new HttpUnauthorizedResult();

            return View(seccion);
        }

        // POST: Secciones/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            var seccion = _repo.FindById(id);
            if (seccion == null)
                return HttpNotFound();
            if (!ValidarPresidente(seccion.Evento))
                return new HttpUnauthorizedResult();
            var eventoId = seccion.Evento.Id;
            _repo.Delete(seccion);
            return RedirectToAction("Index", new { id = eventoId });
        }

        private bool ValidarPresidente(Evento evento)
        {
            return evento.Presidente.UsuarioId == User.Identity.GetUserId();
        }
    }
}
