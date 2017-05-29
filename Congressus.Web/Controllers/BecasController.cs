using Congressus.Web.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Congressus.Web.Controllers
{

    public class BecasController : Controller
    {
        private readonly EventosRepository EventosRepository = new EventosRepository();
        [Authorize(Roles = "admin, presidente")]
        public ActionResult Listado(int EventoId)
        {
            return View();
        }

        [Route("inscripcion/{EventoId:int}")]
        public ActionResult Crear(int EventoId)
        {
            var evento = EventosRepository.FindById(EventoId);
            if (evento == null)
                return HttpNotFound();

            var model = new FormularioBecaViewModel(evento);
            return View(model);
        }
    }
}