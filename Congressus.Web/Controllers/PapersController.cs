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
using Microsoft.AspNet.Identity;
using Congressus.Web.Context;
using Congressus.Web.Repositories;

namespace Congressus.Web.Controllers
{

    public class PapersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly PaperRepository _repo = new PaperRepository();
        private readonly EventosRepository _eventRepo = new EventosRepository();
        //// GET: EnviarPaper
        //[Authorize(Roles ="admin, autor")]
        //public ActionResult EnviarPaper()
        //{
        //    return View();
        //}



        [Authorize]
        // GET: Papers
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            List<Paper> papers;
            if (User.IsInRole("autor"))
            {
                var autor = db.Autores.Single(a => a.UsuarioId == userId);
                papers = autor.Papers.ToList();
                return View(papers);
            }
            if(User.IsInRole("MiembroComite") || User.IsInRole("presidente"))
            {
                var miembro = db.Miembros.Single(m => m.UsuarioId == userId);
                papers = miembro.Papers.ToList();
                return View(papers);
            }
            papers = db.Papers.Include(p => p.Evaluacion).ToList();
            return View(papers);
        }

        [Authorize]
        // GET: Papers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paper paper = db.Papers.Find(id);
            if (paper == null)
            {
                return HttpNotFound();
            }
            return View(paper);
        }
        
        // GET: Papers/Create
        [Authorize(Roles = "admin, autor")]
        public ActionResult Create(int eventoId)
        {
            string mensajeError;
            var model =_repo.CrearPaperVm(eventoId, out mensajeError);
            if (model == null)
            {
                ViewBag.Mensaje = mensajeError;
                return View("Error");
            }
            return View(model);
        }

        // POST: Papers/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, autor")]
        public ActionResult Create(PaperViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var autor = db.Autores.First(a => a.UsuarioId == userId);
                var evento = db.Eventos.Find(model.EventoId);
                var paper = new Paper()
                {
                    Nombre = model.Nombre,
                    AreaCientifica = model.AreaCientifica,
                    Descripcion = model.Descripcion,
                    Fecha = model.Fecha,
                    Path = model.Path,
                    Autor = autor,
                    Evento = evento,
                    CoAutores = model.CoAutores
                };
                var evaluador = evento.Comite.FirstOrDefault(x => x.AreaCientifica == paper.AreaCientifica);
                if (evaluador != null)
                    paper.Evaluador = evaluador;

                evento.Papers.Add(paper);
                autor.Papers.Add(paper);
                db.Papers.Add(paper);                                                                                                         
                db.SaveChanges();
                
                if (model.Archivo != null) { 
                    paper.GuardarArchivo(model.Archivo);
                    //Se modifica la variable path al guardar el archivo.
                    db.Entry(paper).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(model);
        }
        [Authorize(Roles = "admin, autor")]
        // GET: Papers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paper paper = db.Papers.Find(id);
            if (paper == null)
            {
                return HttpNotFound();
            }
            //ViewBag.Id = new SelectList(db.Evaluacions, "Id", "Comentario", paper.Id);
            var model = new PaperViewModel()
            {
                Id = paper.Id,
                EventoId = paper.Evento.Id,
                Descripcion = paper.Descripcion,
                Fecha = paper.Fecha,
                AreasCientifica = new SelectList(paper.Evento.AreasCientificas.Split(';').AsEnumerable()),
                Autor = paper.Autor,
                CoAutores = paper.CoAutores,
                Nombre = paper.Nombre,
            };
            if(!string.IsNullOrEmpty(paper.Path)) model.Path = paper.Path.Split('\\').Last();
            return View(model);
        }

        // POST: Papers/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin, autor")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PaperViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                Paper paper = db.Papers.Find(model.Id);
                if (model.Archivo != null)
                {
                    paper.BorrarArchivo();
                    paper.GuardarArchivo(model.Archivo);
                }
                paper.Nombre = model.Nombre;
                paper.AreaCientifica = model.AreaCientifica;
                paper.Fecha = model.Fecha;
                paper.Path = model.Path;
                paper.Descripcion = model.Descripcion;
                paper.CoAutores = model.CoAutores;

                db.Entry(paper).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Evaluacions, "Id", "Comentario", model.Id);
            return View(model);
        }
        [Authorize(Roles = "admin, autor")]
        // GET: Papers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paper paper = db.Papers.Find(id);
            if (paper == null)
            {
                return HttpNotFound();
            }
            return View(paper);
        }
        [Authorize(Roles = "admin, autor")]
        // POST: Papers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Paper paper = db.Papers.Find(id);
            paper.DeletePaper(db);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin, presidente")]
        // POST: Papers/AssignarMiembro
        [HttpPost]
        public ActionResult AsignarMiembro(AsignarMiembroViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var miembro = db.Miembros.Find(vm.miembroId);
                var paper = db.Papers.Find(vm.paperId);
                if(miembro!=null && paper != null) { 
                    miembro.Papers.Add(paper);
                    paper.Evaluador = miembro;
                    db.Entry(paper).State = EntityState.Modified;
                    db.Entry(miembro).State = EntityState.Modified;
                    db.SaveChanges();
                    return Redirect("/Papers/Details/" + paper.Id);
                }                  
            }
            return Redirect("/Papers/Details/" + vm.paperId);
        }

        public ActionResult DescargarArchivo(int id)
        {
            var paper = db.Papers.Find(id);
            return File(paper.Archivo, System.Net.Mime.MediaTypeNames.Application.Octet, paper.NombreArchivo());
        }

        //GET: /Papers/AceptarRechazar/5
        [Authorize(Roles ="admin, presidente")]
        public ActionResult AceptarRechazar(int Id, string accion)
        {
            var paper = db.Papers.Find(Id);
            if (!User.IsInRole("admin"))
            {
                if (paper.Evento.Presidente.UsuarioId != User.Identity.GetUserId())
                {
                    return new HttpUnauthorizedResult();
                }
            }
            
            switch (accion)
            {
                case "Aceptar":
                    paper.Estado = "Aceptado";
                    break;
                case "Rechazar":
                    paper.Estado = "Rechazado";
                    break;
                default:
                    ViewBag.Codigo = 400;
                    ViewBag.Mensaje = "El parametro Accion solo puede tomar los valores 'Aceptar' o 'Rechazar'";
                    return View("Error");
            }
            db.Entry(paper).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Details","Papers", new { id = paper.Id});
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
