using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Congressus.Web.Models;
using Congressus.Web.Models.Entities;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;
using Microsoft.Reporting.WebForms;
using Congressus.Web.Context;
using Congressus.Web.Repositories;
using Congressus.Web.Attributes;

namespace Congressus.Web.Controllers
{
    //[Authorize(Roles ="presidente, admin")]    
    public class EventosController : Controller
    {
        //private readonly ApplicationDbContext db = new ApplicationDbContext();
        private readonly EventosRepository _repo = new EventosRepository();

        //[Authorize(Roles = "asistente, autor, admin")]
        public ActionResult BuscarEvento()
        {
            return View(_repo.GetAll());
        }

        //[Authorize(Roles = "autor, admin, asistente")]
        [HttpPost]
        public ActionResult BuscarEvento(string patron)
        {
            if (patron.IsNullOrWhiteSpace()) {
                return View(_repo.GetAll());
            }            
            return View(_repo.FindByPattern(patron).ToList());
        }

        [Authorize(Roles = "presidente, admin")] 
        public ActionResult Administrar(int id)
        {
            var model = _repo.FindById(id);
               
            return View(model);
        }
        [Authorize(Roles = "presidente, admin")]
        public ActionResult AgregarMiembro(int miembroId, int eventoId)
        {
            _repo.AgregarMiembroComite(miembroId, eventoId);
            return RedirectToAction("Administrar/" + eventoId);
        }
        [Authorize(Roles = "presidente, admin")]
        public ActionResult RetirarMiembro(int miembroId, int eventoId)
        {
            _repo.RetirarMiembroComite(miembroId, eventoId);
            return RedirectToAction("Administrar/" + eventoId);
        }

        #region EVENTO CRUD

        // GET: Eventos
        [Authorize(Roles = "presidente, MiembroComite, admin, autor")]
        public ActionResult Index()
        {

            if (User.IsInRole("MiembroComite") || User.IsInRole("presidente"))
            {
                var userId = User.Identity.GetUserId();
                var model = _repo.FindByMiembroUserId(userId);
                return View(model);
            }
            if (User.IsInRole("autor"))
            {
                var userId = User.Identity.GetUserId();//buscar id usuario
                var eventos = _repo.FindByAutorUserId(userId);
                return View(eventos.ToList());
            }
            return View(_repo.GetAll());
        }

        // GET: Eventos/Details/5
        public ActionResult Details(int id)
        {
            var evento = _repo.FindById(id);
            if (evento == null)
                return HttpNotFound();

            return View(evento);
        }

        // GET: Eventos/Create
        [Authorize(Roles = "presidente, admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Eventos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "presidente, admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventoViewModel model)
        {

            if (ModelState.IsValid)
            {
                var evento = model.ToEvento();
                var userId = User.Identity.GetUserId();
                if (!User.IsInRole("admin"))
                {
                    _repo.CrearEventoByOrganizador(evento, userId);
                }
                else
                {
                    _repo.CrearEventoByAdmin(evento, userId);
                }
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Eventos/Edit/5
        [PresidenteAuthorize(Roles = "presidente, admin")]
        public ActionResult Edit(int id)
        {
            Evento evento = _repo.FindById(id);
            if (evento == null)
                return HttpNotFound();
            var model = new EventoViewModel(evento);
            return View(model);
        }

        // POST: Eventos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PresidenteAuthorize(Roles = "presidente, admin")]
        public ActionResult Edit(EventoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var evento = _repo.FindById(model.Id);
                if (evento == null)
                    return HttpNotFound();
                evento = model.ToEvento(evento);
                _repo.Edit(evento);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Eventos/Delete/5
        [Authorize(Roles = "presidente, admin")]
        public ActionResult Delete(int id)
        {
            Evento evento = _repo.FindById(id);
            if (evento == null)
                return HttpNotFound();

            return View(evento);
        }

        // POST: Eventos/Delete/5
        [Authorize(Roles = "presidente, admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _repo.EliminarEvento(id);
            return RedirectToAction("Index");
        } 
        #endregion

        [Authorize(Roles = "admin, presidente")]
        public ActionResult ComiteEvaluador(int id)
        {
            var evento = _repo.FindById(id);
            if (!ValidarPresidente(evento))
                return View("Error");

            //Todos los miembros de los eventos del usuario del presidente.
            var userId = evento.Presidente.UsuarioId;
            ViewBag.miembros = _repo.MiembrosDeTodosLosEventos(userId);
            evento.Comite.Remove(evento.Presidente);

            return View(evento);
        }

        [Authorize(Roles = "admin, presidente")]
        public ActionResult Certificados(int id)
        {
            var evento = _repo.FindById(id);
            if (!ValidarPresidente(evento))
                return View("Error");
            return View(evento);
        }

        #region INICIO
        [Authorize(Roles = "admin, presidente")]
        public ActionResult ConfiguracionInicio(int id)
        {
            var evento = _repo.FindById(id);
            if (!ValidarPresidente(evento))
                return View("Error");
            return View(evento);
        }

        [Authorize(Roles = "admin, presidente")]
        public ActionResult SubirLogo(ImagenesUploadVM model)
        {
            var evento = _repo.FindById(model.Id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                var pathFile = _repo.GuardarImagenes(model, "Logo").First();
                evento.LogoPath = pathFile;
                _repo.Edit(evento);
                return RedirectToAction("Administrar", new { id = model.Id });
            }
            return View("ConfiguracionInicio", evento);
        }

        [Authorize(Roles = "admin, presidente")]
        public ActionResult SubirPrograma(PdfUploadVM model)
        {
            var evento = _repo.FindById(model.Id);
            if (evento == null)
                return HttpNotFound();
            if (ModelState.IsValid)
            {
                var relPath = "/Content/Files/Eventos" + model.Id + "/Programa/";
                var pathFile = _repo.GuardarArchivos(model.Documentos, relPath).First();
                evento.ProgramaPath = pathFile;
                _repo.Edit(evento);
                return RedirectToAction("Administrar", new { id = model.Id });
            }
            return View("Administrar", evento);
        }
        public ActionResult Programa(int id)
        {
            var evento = _repo.FindById(id);
            if (evento == null) return HttpNotFound();
            if (evento.ProgramaPath == null) return HttpNotFound();
            if (!System.IO.File.Exists(Server.MapPath(evento.ProgramaPath))) return HttpNotFound();

            var fs = new FileStream(Server.MapPath(evento.ProgramaPath), FileMode.Open);
            return File(fs, "application/pdf");
            
        }
        [Authorize(Roles = "admin, presidente")]
        public ActionResult SubirImagenesInicio(ImagenesUploadVM model)
        {
            var evento = _repo.FindById(model.Id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                var imagenes = _repo.GuardarImagenes(model, "Inicio");
                evento.ImagenesInicio = string.Join(";", imagenes);
                _repo.Edit(evento);
                return RedirectToAction("Administrar", new { id = model.Id });
            }
            return View("ConfiguracionInicio", evento);

        }
        [Authorize(Roles = "admin, presidente")]
        public ActionResult SubirImagenesSponsors(ImagenesUploadVM model)
        {
            var evento = _repo.FindById(model.Id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                var imagenes = _repo.GuardarImagenes(model, "Sponsors");
                evento.ImagenesSponsors = string.Join(";", imagenes);
                _repo.Edit(evento);
                return RedirectToAction("Administrar", new { id = model.Id });
            }
            return View("ConfiguracionInicio", evento);


        }
        [Authorize(Roles = "admin, presidente")]
        public ActionResult SubirTextoInicio(TextoInicioVM model)
        {
            var evento = _repo.FindById(model.Id);
            if (evento != null)
            {
                if (ModelState.IsValid)
                {
                    evento.TextoBienvenida = model.TextoBienvenida;
                    _repo.Edit(evento);
                }
            }
            return RedirectToAction("Administrar", new { id = model.Id });
        }
        #endregion
        public bool ValidarPresidente(Evento evento)
        {
            return evento.Presidente.UsuarioId == User.Identity.GetUserId();
        }

        [Authorize(Roles = "presidente, admin")]
        public ActionResult AsignarAreaAMiembro(int AreaId)
        {
            var area = _repo.FindAreaById(AreaId);
            if (area == null)
                return HttpNotFound();

            var model = new AreaCientifiaViewModel(area);

            return View(model);
        }
        [Authorize(Roles = "presidente, admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AsignarAreaAMiembro(AreaCientifiaViewModel model)
        {
            var area = _repo.FindAreaById(model.Id);
            if (model == null)
                return HttpNotFound();
            if (ModelState.IsValid)
            {
                if (_repo.AsignarAreaAMiembro(model))
                    return RedirectToAction("Administrar", new { id = model.EventoId });
            }

            model = new AreaCientifiaViewModel(area);

            return View(model);
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
