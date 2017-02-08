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
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Congressus.Web.Context;

namespace Congressus.Web.Controllers
{
    [Authorize]
    public class AutoresController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Autores
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            var autores = db.Autores.Include(a => a.Usuario);
            return View(autores.ToList());
        }

        // GET: Autores/Details/5
        [Authorize(Roles = "admin, autor")]
        public ActionResult Details(int? id)
        {
            Autor autor;
            if (User.IsInRole("autor"))
            {
                var userId = User.Identity.GetUserId();
                autor = db.Autores.First(a => a.UsuarioId == userId);
                return View(autor);
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            autor = db.Autores.Find(id);
            if (autor == null)
            {
                return HttpNotFound();
            }
            return View(autor);
        }

        // GET: Autores/Create
        [Authorize(Roles ="admin")]
        public ActionResult Create()
        {

            using (var UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>()))
            {
                ViewBag.UsuarioId = new SelectList(UserManager.Users, "Id", "Email");                
            }                             
            return View();
        }

        // POST: Autores/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Autor autor)
        {
            if (ModelState.IsValid)
            {
                db.Autores.Add(autor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            using (var UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>()))
            {
                ViewBag.UsuarioId = new SelectList(UserManager.Users, "Id", "Email");
            }
            return View(autor);
        }

        // GET: Autores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Autor autor = db.Autores.Find(id);
            if (autor == null)
            {
                return HttpNotFound();
            }
            using (var UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>()))
            {
                ViewBag.UsuarioId = new SelectList(UserManager.Users, "Id", "Email");
            }
            return View(autor);
        }

        // POST: Autores/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Autor autor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(autor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details");
            }
            using (var UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>()))
            {
                ViewBag.UsuarioId = new SelectList(UserManager.Users, "Id", "Email");
            }
            return View(autor);
        }

        // GET: Autores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Autor autor = db.Autores.Find(id);
            if (autor == null)
            {
                return HttpNotFound();
            }
            return View(autor);
        }

        // POST: Autores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Autor autor = db.Autores.Find(id);
            db.Autores.Remove(autor);
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
