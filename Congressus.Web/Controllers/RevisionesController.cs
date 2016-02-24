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
    public class RevisionesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Revisiones
        //public ActionResult Index()
        //{
        //    return View(db.Revisiones.ToList());
        //}

        // GET: Revisiones/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Revision revision = db.Revisiones.Find(id);
        //    if (revision == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(revision);
        //}

        // GET: Revisiones/Create
        public ActionResult Create(int paperId)
        {
            ViewBag.paper = db.Papers.Find(paperId);
            return View();
        }

        // POST: Revisiones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RevisionViewModel RevisionVm)
        {
            if (ModelState.IsValid)
            {
                var userid = User.Identity.GetUserId();
                var miembro = db.Miembros.Single(m => m.UsuarioId == userid);
                var paper = db.Papers.Find(RevisionVm.PaperId);

                var revision = new Revision {
                    Comentario = RevisionVm.Comentario,
                    Estado = RevisionVm.Estado,
                    MiembroComite = miembro,
                    Path = RevisionVm.Path,
                    Paper = paper
                };
                if(RevisionVm.Archivo!=null)
                    revision.GuardarArchivo(RevisionVm.Archivo);

                paper.Revisiones.Add(revision);                
                db.Revisiones.Add(revision);

                db.Entry(paper).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details","Papers", new { Id = paper.Id});
            }

            return View(RevisionVm);
        }

        // GET: Revisiones/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Revision revision = db.Revisiones.Find(id);
        //    if (revision == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(revision);
        //}

        // POST: Revisiones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Estado,Comentario,Path")] Revision revision)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(revision).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(revision);
        //}

        // GET: Revisiones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Revision revision = db.Revisiones.Find(id);
            if (revision == null)
            {
                return HttpNotFound();
            }
            return View(revision);
        }

        // POST: Revisiones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Revision revision = db.Revisiones.Find(id);
            db.Revisiones.Remove(revision);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: Revisiones/Descargar/5
        public ActionResult DescargarArchivo(int id) {
            var revision = db.Revisiones.Find(id);
            return File(revision.Archivo, System.Net.Mime.MediaTypeNames.Application.Octet, revision.NombreArchivo());
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
