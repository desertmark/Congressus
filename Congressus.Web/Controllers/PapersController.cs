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

namespace Congressus.Web.Controllers
{

    public class PapersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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
            ViewBag.EventoId = eventoId;
            return View();
        }

        // POST: Papers/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, autor")]
        public ActionResult Create(PaperViewModel paperVm, int eventoId)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var autor = db.Autores.First(a => a.UsuarioId == userId);
                var evento = db.Eventos.Find(eventoId);
                var paper = new Paper()
                {
                    Nombre = paperVm.Nombre,
                    AreaCientifica = paperVm.AreaCientifica,
                    Descripcion = paperVm.Descripcion,
                    Fecha = paperVm.Fecha,
                    Path = paperVm.Path,
                    Autor = autor,
                    Evento = evento
                };
                evento.Papers.Add(paper);
                evento.Presidente.Papers.Add(paper);
                autor.Papers.Add(paper);
                db.Papers.Add(paper);                                                                                                         
                db.SaveChanges();
                //Se guarda para y carga de nuevo el mismo paper para poder tener el valor del id.
                paper = db.Papers.Single(p => p.Nombre == paper.Nombre && p.Fecha == paper.Fecha && p.Autor.Id == paper.Autor.Id);
                paper.GuardarArchivo(paperVm.Archivo);
                //Se modifica la variable path al guardar el archivo.
                db.Entry(paper).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(paperVm);
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
            ViewBag.Id = new SelectList(db.Evaluacions, "Id", "Comentario", paper.Id);
            return View(paper);
        }

        // POST: Papers/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin, autor")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PaperViewModel paperVm)
        {
            
            if (ModelState.IsValid)
            {
                Paper paper = db.Papers.Find(paperVm.Id);
                if (paperVm.Archivo != null)
                {
                    paper.BorrarArchivo();
                    paper.GuardarArchivo(paperVm.Archivo);
                }
                paper.Nombre = paperVm.Nombre;
                paper.AreaCientifica = paperVm.AreaCientifica;
                paper.Fecha = paperVm.Fecha;
                paper.Path = paperVm.Path;
                paper.Descripcion = paperVm.Descripcion;
                

                db.Entry(paper).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Evaluacions, "Id", "Comentario", paperVm.Id);
            return View(paperVm);
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
