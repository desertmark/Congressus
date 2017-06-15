using Congressus.Web.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Congressus.Web.Controllers
{
    [Authorize]
    public class BecasController : Controller
    {
        private readonly EventosRepository EventosRepository = new EventosRepository();
        private readonly BecasRepository BecasRepository = new BecasRepository();
        [Authorize(Roles = "admin, presidente")]
        public ActionResult Listado(int EventoId)
        {
            var becas = BecasRepository.GetByEventoId(EventoId);
            return View(becas);
        }

        [Authorize(Roles = "asistente, admin")]
        public ActionResult Crear(int EventoId)
        {
            var evento = EventosRepository.FindById(EventoId);
            if (evento == null)
                return HttpNotFound();

            var model = new FormularioBecaViewModel(evento);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "asistente, admin")]
        public ActionResult Crear(FormularioBecaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var evento = EventosRepository.FindById(model.EventoId);
                model.SetearSelectLists(evento);
                return View(model);
            }
            if (!BecasRepository.Add(model))
                return View("Error");
            return RedirectToAction("Details", "Eventos", new { Id = model.EventoId});

        }
    }
}