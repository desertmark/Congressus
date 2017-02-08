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

namespace Congressus.Web.Controllers
{
    [Authorize(Roles = "presidente, admin, MiembroComite")]
    public class EvaluacionesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Evaluaciones
        //public ActionResult Index()
        //{
        //    var evaluacions = db.Evaluacions.Include(e => e.Paper);
        //    return View(evaluacions.ToList());
        //}

        // GET: Evaluaciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evaluacion evaluacion = db.Evaluacions.Find(id);
            if (evaluacion == null)
            {
                return HttpNotFound();
            }
            return View(evaluacion);
        }

        // GET: Evaluaciones/Create
        public ActionResult Create(int paperid)
        {
            ViewBag.paper = db.Papers.Find(paperid);
            return View();
        }

        // POST: Evaluaciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EvaluacionesViewModel evaluacionVm)
        {
            if (ModelState.IsValid)
            {
                var paper = db.Papers.Find(evaluacionVm.paperId);
                if(paper == null)
                {
                    return HttpNotFound("El paper con el id "+ evaluacionVm.paperId + " no fue encontrado");
                }
                
                var userid = User.Identity.GetUserId();
                if(paper.Evaluador.UsuarioId != userid)
                {
                    return new HttpUnauthorizedResult();
                }

                var miembro = db.Miembros.Single(m => m.UsuarioId == userid);

                Evaluacion evaluacion = new Evaluacion
                {
                    Calificacion = evaluacionVm.Calificacion,
                    Paper = paper,
                    Comentario = evaluacionVm.Comentario,
                    MiembroComite = miembro                    
                };
                db.Evaluacions.Add(evaluacion);
                db.SaveChanges();
                return RedirectToAction("Details","Papers", new { Id = evaluacionVm.paperId});
            }

            ViewBag.paper = db.Papers.Find(evaluacionVm.paperId);
            return View(evaluacionVm);
        }

        // GET: Evaluaciones/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Evaluacion evaluacion = db.Evaluacions.Find(id);
        //    if (evaluacion == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.Id = new SelectList(db.Papers, "Id", "Nombre", evaluacion.Id);
        //    return View(evaluacion);
        //}

        // POST: Evaluaciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Calificacion,Comentario")] Evaluacion evaluacion)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(evaluacion).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.Id = new SelectList(db.Papers, "Id", "Nombre", evaluacion.Id);
        //    return View(evaluacion);
        //}

        // GET: Evaluaciones/Delete/5
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Evaluacion evaluacion = db.Evaluacions.Find(id);

            var userid = User.Identity.GetUserId();
            if (evaluacion.Paper.Evaluador.UsuarioId != userid)
            {
                return new HttpUnauthorizedResult();
            }

            if (evaluacion == null)
            {
                return HttpNotFound();
            }
            return View(evaluacion);
        }

        // POST: Evaluaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Evaluacion evaluacion = db.Evaluacions.Find(id);

            var userid = User.Identity.GetUserId();
            if (evaluacion.Paper.Evaluador.UsuarioId != userid)
            {
                return new HttpUnauthorizedResult();
            }

            var paperid = evaluacion.Paper.Id;
            db.Evaluacions.Remove(evaluacion);
            db.SaveChanges();
            return RedirectToAction("Details", "Papers", new { Id = paperid });
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
