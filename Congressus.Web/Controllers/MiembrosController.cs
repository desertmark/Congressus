using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Congressus.Web.Models;
using Congressus.Web.Models.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Congressus.Web.Context;

namespace Congressus.Web.Controllers
{
    [Authorize]
    public class MiembrosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Miembros
        [Authorize(Roles ="admin, presidente")]
        public ActionResult Index()
        {
            List<MiembroComite> miembros;
            if (User.IsInRole("presidente"))
            {
                var userid = User.Identity.GetUserId();
                var presidente = db.Miembros.Single(m => m.UsuarioId == userid);
                miembros = new List<MiembroComite>();
                foreach (var ev in presidente.Eventos)
                {
                    miembros.AddRange(ev.Comite);
                }
                miembros = miembros.Distinct().ToList();
            }else { 
                miembros = db.Miembros.Include(m => m.Usuario).ToList();
            }
            return View(miembros);
        }

        //[Authorize(Roles="presidente, MiembroComite")]
        //// GET: Miembros/Details
        //public ActionResult Details()
        //{
        //    MiembroComite miembroComite = db.Miembros.Single(m => m.UsuarioId == User.Identity.GetUserId());
        //    if (miembroComite == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(miembroComite);
        //}

        // GET: Miembros/Details/5
        [Authorize(Roles = "admin, presidente, MiembroComite")]
        public ActionResult Details(int? id)
        {
            MiembroComite miembroComite;
            if (id == null)
            {
                var userId = User.Identity.GetUserId();
                miembroComite = db.Miembros.Single(m => m.UsuarioId == userId);
                if (miembroComite == null)
                {
                    return HttpNotFound();
                }
                return View(miembroComite);
            }

            if (!User.IsInRole("admin"))
            {
                return new HttpUnauthorizedResult();
            }

            miembroComite = db.Miembros.Find(id);
            if (miembroComite == null)
            {
                return HttpNotFound();
            }
            return View(miembroComite);
        }
        [Authorize(Roles="presidente, admin")]
        // GET: Miembros/Create
         public ActionResult Create()
        {
            if(!User.IsInRole("admin"))
            {
                var userId = User.Identity.GetUserId();
                var eventos = db.Miembros.Single(m => m.UsuarioId == userId).Eventos.ToList();
                ViewBag.Eventos =  new SelectList(eventos, "Id", "Nombre");
            }
            else
            {
                var eventos = db.Eventos.ToList();
                var selectList = new SelectList(eventos, "Id", "Nombre");
                ViewBag.Eventos = selectList;
            }
            
            return View();
        }

        // POST: Miembros/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "presidente, admin")]
        public ActionResult Create(CrearMiembroViewModel model)
        {
            if (ModelState.IsValid)
            {
                var store = new UserStore<ApplicationUser>(db);
                var UserManager = new ApplicationUserManager(store);

                var usuario = new ApplicationUser()
                {
                    UserName = model.Email,
                    Email = model.Email,
                };

                var miembro = new MiembroComite()
                {
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    AreaCientifica = model.AreaCientifica,               
                };
                var result = UserManager.CrearMiembro(usuario, model.Password, miembro);
                if (result.Succeeded) { 
                    var userId = UserManager.FindByEmail(model.Email).Id;
                    miembro = db.Miembros.First(m => m.UsuarioId == userId);

                    var evento = db.Eventos.Find(model.EventoId);
                    evento.Comite.Add(miembro);
                    miembro.Eventos.Add(evento);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        // GET: Miembros/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MiembroComite miembroComite = db.Miembros.Find(id);
            if (miembroComite == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsuarioId = new SelectList(db.Users, "Id", "Email", miembroComite.UsuarioId);
            return View(miembroComite);
        }

        // POST: Miembros/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MiembroComite miembroComite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(miembroComite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details/" + miembroComite.Id);
            }
            ViewBag.UsuarioId = new SelectList(db.Users, "Id", "Email", miembroComite.UsuarioId);
            return View(miembroComite);
        }

        // GET: Miembros/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MiembroComite miembroComite = db.Miembros.Find(id);
            if (miembroComite == null)
            {
                return HttpNotFound();
            }
            return View(miembroComite);
        }

        // POST: Miembros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MiembroComite miembroComite = db.Miembros.Find(id);

            miembroComite.Papers.ToList().ForEach(p => p.DeletePaper(db));
            db.Papers.RemoveRange(miembroComite.Papers);

            miembroComite.Papers.Clear();
            db.Miembros.Remove(miembroComite);
            db.SaveChanges();
            return RedirectToAction("Index");
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
