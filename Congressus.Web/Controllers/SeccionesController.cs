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
            return View(evento.Secciones);
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
        public ActionResult Create(SeccionEventoVM model)
        {
            if (ModelState.IsValid)
            {
                var evento = _repo.FindEventoById(model.EventoId);
                if(evento!=null)
                    return HttpNotFound();
                if (!ValidarPresidente(evento))
                    return new HttpUnauthorizedResult();

                var seccion = _repo.GetSeccionFromVM(model, evento);
                _repo.Add(seccion);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Secciones/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Secciones/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Secciones/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Secciones/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private bool ValidarPresidente(Evento evento)
        {
            return evento.Presidente.UsuarioId == User.Identity.GetUserId();
        }
    }
}
