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

namespace Congressus.Web.Controllers
{
    //[Authorize(Roles ="presidente, admin")]    
    public class EventosController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "autor, admin")]
        public ActionResult BuscarEvento()
        {
            return View(db.Eventos);
        }

        [Authorize(Roles = "autor, admin")]
        [HttpPost]
        public ActionResult BuscarEvento(string patron)
        {
            if (patron.IsNullOrWhiteSpace()) {
                return View(db.Eventos);
            }
            var eventos = db.Eventos.Where(e => e.Nombre.Contains(patron) || 
                                           e.Lugar.Contains(patron) || 
                                           e.Direccion.Contains(patron) || 
                                           e.Tema.Contains(patron));

            return View(eventos);
        }

        [Authorize(Roles = "presidente, admin")] 
        //GET: Eventos/Administrar/5
        public ActionResult Administrar(int id)
        {
            var model = db.Eventos.Find(id);

            //Todos los miembros de los eventos del usuario con userId
            var userId = User.Identity.GetUserId();
            var eventos = db.Miembros.Single(m => m.UsuarioId == userId).Eventos;
            var miembros = new List<MiembroComite>();
            foreach (var e in eventos)
            {
                miembros = miembros.Concat(e.Comite.ToList()).ToList();
            }
            ViewBag.miembros = miembros.Distinct();
            model.Comite.Remove(model.Presidente);               
            return View(model);
        }
        public ActionResult AgregarMiembro()
        {
            var miembroId = Convert.ToInt16((Request.Form["miembroId"]));
            var eventoId = Convert.ToInt16(Request.Form["eventoId"]);

            var miembro = db.Miembros.Find(miembroId);
            var evento = db.Eventos.Find(eventoId);
            if (miembro.Eventos == null)
            {
                miembro.Eventos = new List<Evento>() {evento};
            }
            else
            {
                miembro.Eventos.Add(evento);
            }
            evento.Comite.Add(miembro);

            db.SaveChanges();
            return RedirectToAction("Administrar/" + eventoId);
        }


        public ActionResult RetirarMiembro()
        {
            var miembroId = Convert.ToInt16((Request.Form["miembroId"]));
            var eventoId = Convert.ToInt16(Request.Form["eventoId"]);

            var miembro = db.Miembros.Find(miembroId);
            var evento = db.Eventos.Find(eventoId);

            miembro.Eventos.Remove(evento);
            evento.Comite.Remove(miembro);

            db.SaveChanges();
            return RedirectToAction("Administrar/" + eventoId);
        }


        // GET: Eventos
        [Authorize(Roles = "presidente, MiembroComite, admin, autor")]  
        public ActionResult Index()
        {
            
            if (User.IsInRole("MiembroComite") || User.IsInRole("presidente"))
            {
                var userId = User.Identity.GetUserId();
                var miembro = db.Miembros.Single(m => m.UsuarioId == userId);
                var model = miembro.Eventos;
                return View(model);
            }
            if(User.IsInRole("autor"))
            {
                var userId = User.Identity.GetUserId();//buscar id usuario
                var autor = db.Autores.Single(a => a.UsuarioId == userId); // buscar autor para ese Id de usuario 
                var eventos = autor.Papers.Select(p => p.Evento).Distinct(); // buscar en la lista de papers de ese autor todos los eventos en que participa habiendo enviado un paper

                return View(eventos.ToList());
            }
            return View(db.Eventos.ToList());
        }

        // GET: Eventos/Details/5
        [Authorize(Roles = "presidente, admin, MiembroComite, autor")] 
        public ActionResult Details(int? id)
        {            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var evento = db.Eventos.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            //var userId = User.Identity.GetUserId();
            //if(evento.Comite.All(m => m.UsuarioId != userId))
            //{
            //    return new HttpUnauthorizedResult();
            //}

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

        public ActionResult Create(Evento evento)
        {
            if (ModelState.IsValid)
            {
                if(!User.IsInRole("admin"))
                { 
                    var userId = User.Identity.GetUserId();
                    evento.Presidente = db.Miembros.Single(p => p.Usuario.Id == userId);
                    evento.Presidente.Eventos.Add(evento);
                }
                else
                {
                    evento.Presidente = new MiembroComite();
                    db.Eventos.Add(evento);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(evento);
        }

        // GET: Eventos/Edit/5
        [Authorize(Roles = "presidente, admin")] 
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evento evento = db.Eventos.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            return View(evento);
        }

        // POST: Eventos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "presidente, admin")] 
        public ActionResult Edit(Evento evento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(evento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(evento);
        }

        // GET: Eventos/Delete/5
        [Authorize(Roles = "presidente, admin")] 
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evento evento = db.Eventos.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            return View(evento);
        }

        // POST: Eventos/Delete/5
        [Authorize(Roles = "presidente, admin")] 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Evento evento = db.Eventos.Find(id);

            evento.Papers.ToList().ForEach(p => p.DeletePaper(db));
            db.Papers.RemoveRange(evento.Papers);

            evento.Comite.Clear();
            db.Eventos.Remove(evento);
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
