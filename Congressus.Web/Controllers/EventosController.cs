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

namespace Congressus.Web.Controllers
{
    //[Authorize(Roles ="presidente, admin")]    
    public class EventosController : Controller
    {
        //private readonly ApplicationDbContext db = new ApplicationDbContext();
        private readonly EventosRepository _repo = new EventosRepository();

        [Authorize(Roles = "asistente, autor, admin")]
        public ActionResult BuscarEvento()
        {
            return View(_repo.GetAll());
        }

        [Authorize(Roles = "autor, admin, asistente")]
        [HttpPost]
        public ActionResult BuscarEvento(string patron)
        {
            if (patron.IsNullOrWhiteSpace()) {
                return View(_repo.GetAll());
            }            
            return View(_repo.FindByPattern(patron).ToList());
        }

        [Authorize(Roles = "presidente, admin")] 
        //GET: Eventos/Administrar/5
        public ActionResult Administrar(int id)
        {
            var model = _repo.FindById(id);

            //Todos los miembros de los eventos del usuario del presidente.
            var userId = model.Presidente.UsuarioId;
            ViewBag.miembros = _repo.MiembrosDeTodosLosEventos(userId);
            model.Comite.Remove(model.Presidente);               
            return View(model);
        }
        public ActionResult AgregarMiembro(int miembroId, int eventoId)
        {
            _repo.AgregarMiembroComite(miembroId, eventoId);
            return RedirectToAction("Administrar/" + eventoId);
        }


        public ActionResult RetirarMiembro(int miembroId, int eventoId)
        {
            _repo.RetirarMiembroComite(miembroId, eventoId);
            return RedirectToAction("Administrar/" + eventoId);
        }


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
            if(User.IsInRole("autor"))
            {
                var userId = User.Identity.GetUserId();//buscar id usuario
                var eventos = _repo.FindByAutorUserId(userId);
                return View(eventos.ToList());
            }
            return View(_repo.GetAll());
        }

        // GET: Eventos/Details/5
        [Authorize] 
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

        public ActionResult Create(Evento evento)
        {
            if (ModelState.IsValid)
            {
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

            return View(evento);
        }

        // GET: Eventos/Edit/5
        [Authorize(Roles = "presidente, admin")] 
        public ActionResult Edit(int id)
        {
            Evento evento = _repo.FindById(id);
            if (evento == null)
                return HttpNotFound();

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
                _repo.Edit(evento);
                return RedirectToAction("Index");
            }
            return View(evento);
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
        [Authorize(Roles ="presidente, admin")]
        public ActionResult UploadCertificadoAsistentes(int eventoId, HttpPostedFileBase certificado)
        {            
            var extension = certificado.FileName.Split('.').Last();
            if((extension == "rdl") || (extension == "rdlc"))
            {
                var path = GetCertificadosPath("CertificadoAsistencia.rdl");
                _repo.GuardarCertificadoAsistentes(eventoId, path, certificado);
            }
            return RedirectToAction("Administrar",new { Id = 1 });
        }

        [Authorize(Roles ="admin,presidente")]
        public ActionResult VerCertificadoAsistente(int id)
        {
            var evento = _repo.FindById(id);
            if (string.IsNullOrEmpty(evento.CertificadoAsistentesPath))
            {
                return HttpNotFound();
            }

            try {
                var renderedBytes = _repo.RenderizarCertificado(evento.Id, evento.CertificadoAsistentesPath);
                return File(renderedBytes, "application/pdf");
            }
            catch(Exception e)
            {
                ViewBag.Mensaje = e.Message;
                return View("Error");
            }
        }
        [Authorize(Roles ="asistente")]
        public ActionResult GenerarCertificadoAsistente(int id)
        {
            
            var evento = _repo.FindById(id);
            if (string.IsNullOrEmpty(evento.CertificadoAsistentesPath))
            {
                ViewBag.Mensaje = "No se ha cargado ninguna plantilla para la generacion de certificados.";
                return View("Error");
            }
            if (evento == null)
            {
                ViewBag.Mensaje = "Evento no encontrado";
                return View("Error");
            }
            if (!evento.HabilitarDescargaCertificados)
            {
                ViewBag.Mensaje = "Los organizadores todavia no han habilitado la descarga de los certificados.";
                return View("Error");
            }
            try
            {
                var uid = User.Identity.GetUserId();
                var inscripcion = evento.Inscripciones.FirstOrDefault(x => x.Asistente.UsuarioId == uid);
                if (inscripcion == null)
                {
                    ViewBag.Mensaje = "Ud no es un asistente inscripto a este evento.";
                    return View("Error");
                }
                var nombre = inscripcion.Asistente.Nombre + " " + inscripcion.Asistente.Apellido;
                var tempPath = _repo.GenerarCertificado(evento.CertificadoAsistentesPath, nombre, evento.Nombre, evento.FechaFin.ToShortDateString());
                var renderedBytes = _repo.RenderizarCertificado(evento.Id, tempPath);
                return File(renderedBytes, "application/pdf");
            }
            catch (Exception e)
            {
                ViewBag.Mensaje = e.Message;
                return View("Error");
            }
        }
        [Authorize(Roles="admin, presidente")]
        public ActionResult HabilitarDescargaCertificados(int id)
        {
            var evento = _repo.FindById(id);
            if (evento == null)
            {
                ViewBag.Mensaje = "Evento no encontrado.";
                return View("Error");
            }
            _repo.HabilitarDeshabilitarCertificados(evento, true);
            return RedirectToAction("Administrar", new { id = evento.Id });
        }
        [Authorize(Roles = "admin, presidente")]
        public ActionResult DeshabilitarDescargaCertificados(int id)
        {
            var evento = _repo.FindById(id);
            if (evento == null)
            {
                ViewBag.Mensaje = "Evento no encontrado.";
                return View("Error");
            }
            _repo.HabilitarDeshabilitarCertificados(evento, false);
            return RedirectToAction("Administrar",new { id = evento.Id});
        }
        [Authorize(Roles ="admin, presidente")]
        public ActionResult UploadCertificadoOradores(int eventoId, HttpPostedFileBase certificado)
        {
            var extension = certificado.FileName.Split('.').Last();
            if ((extension == "rdl") || (extension == "rdlc"))
            {
                var path = GetCertificadosPath("CertificadoOradores.rdl");
                _repo.GuardarCertificadoOradores(eventoId, path, certificado);
            }
            return RedirectToAction("Administrar", new { Id = 1 });
        }
        [Authorize(Roles = "admin,presidente")]
        public ActionResult VerCertificadoOradores(int id)
        {
            var evento = _repo.FindById(id);
            if (string.IsNullOrEmpty(evento.CertificadoOradoresPath))
            {
                return HttpNotFound();
            }
            try { 
                var renderedBytes = _repo.RenderizarCertificado(evento.Id,evento.CertificadoOradoresPath);
                return File(renderedBytes, "application/pdf");
            }catch (Exception e)
            {
                ViewBag.Mensaje = e.Message;
                return View("Error");
            }
        }

        [Authorize(Roles = "autor")]
        public ActionResult GenerarCertificadoOradores(int id, int charlaId)
        {            
            var evento = _repo.FindById(id);
            if (string.IsNullOrEmpty(evento.CertificadoAsistentesPath))
            {
                ViewBag.Mensaje = "No se ha cargado ninguna plantilla para la generacion de certificados.";
                return View("Error");
            }
            if (evento == null)
            {
                ViewBag.Mensaje = "Evento no encontrado";
                return View("Error");
            }
            if (!evento.HabilitarDescargaCertificados)
            {
                ViewBag.Mensaje = "Los organizadores todavia no han habilitado la descarga de los certificados.";
                return View("Error");
            }
            try
            {
                var uid = User.Identity.GetUserId();
                var charla = evento.Charlas.First(x => x.Orador.UsuarioId == uid && x.Id == charlaId);
                if (charla == null)
                {
                    ViewBag.Mensaje = "No se ha encontrado la charla, o ésta no lo tiene a usted como orador.";
                    return View("Error");
                }
                var nombre = charla.Orador.Nombre + " " + charla.Orador.Apellido;
                var tempPath = _repo.GenerarCertificado(evento.CertificadoOradoresPath, nombre, evento.Nombre, evento.FechaFin.ToShortDateString(),charla.Titulo);
                var renderedBytes = _repo.RenderizarCertificado(evento.Id, tempPath);
                return File(renderedBytes, "application/pdf");
            }
            catch (Exception e)
            {
                ViewBag.Mensaje = e.Message;
                return View("Error");
            }
        }

        private string GetCertificadosPath(string certificadoName)
        {
            string path;
            var username = User.Identity.GetUserName();
            path = Path.Combine(Server.MapPath("/Content/Files"), username, "Certificados");
            Directory.CreateDirectory(path);
            path = Path.Combine(path, certificadoName);
            return path;
        }
        [Authorize(Roles = "admin, presidente")]
        public ActionResult EliminarCertificadoAsistentes(int id)
        {
            var evento = _repo.FindById(id);
            if (evento == null)
            {
                ViewBag.Mensaje = "Evento no encontrado.";
                return View("Error");
            }
            _repo.EliminarCertificadoAsistentes(evento);
            return RedirectToAction("Administrar", new { id = id });
        }
        [Authorize(Roles = "admin, presidente")]
        public ActionResult EliminarCertificadoOradores(int id)
        {
            var evento = _repo.FindById(id);
            if (evento == null)
            {
                ViewBag.Mensaje = "Evento no encontrado.";
                return View("Error");
            }
            _repo.EliminarCertificadoOradores(evento);
            return RedirectToAction("Administrar", new { id = id });
        }
        [Authorize(Roles = "presidente")]
        public ActionResult SubirLogo(LogoVM model)
        {
            var evento = _repo.FindById(model.Id);
            var path = "/Content/Files/Images/Eventos/" + evento.Id + "/Logo/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(Server.MapPath(path));
            }
            var pathFile = path + model.Logo.FileName;
            model.Logo.SaveAs(Server.MapPath(pathFile));
            evento.LogoPath = pathFile;
            _repo.Edit(evento);
            return RedirectToAction("Administrar", new { id = model.Id });
        }

        [Authorize(Roles = "presidente")]
        public ActionResult SubirImagenesInicio(ImagenesInicioVM model)
        {
            var evento = _repo.FindById(model.Id);
            if (evento != null)
            {
                var path = "/Content/Files/Images/Eventos/" + model.Id + "/Inicio/";
                if (ModelState.IsValid)
                {
                    if (!Directory.Exists(Server.MapPath(path)))
                        Directory.CreateDirectory(Server.MapPath(path));
                    var imagenes = new List<string>();
                    foreach (var imagen in model.Imagenes)
                    {
                        var filePath = path + imagen.FileName;
                        imagen.SaveAs(Server.MapPath(filePath));
                        imagenes.Add(filePath);
                    }
                    evento.ImagenesInicio = string.Join(";", imagenes);
                    _repo.Edit(evento);
                }
            }
            
            return RedirectToAction("Administrar", new { id = model.Id });
        }
        public ActionResult SubirTextoInicio(TextoInicioVM model)
        {
            var evento = _repo.FindById(model.Id);
            if (evento != null)
            {
                if (ModelState.IsValid)
                {
                    evento.TextoBienvenida = model.Texto;
                    _repo.Edit(evento);
                }
            }
            return RedirectToAction("Administrar", new { id = model.Id });
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
