using Congressus.Web.Models;
using Congressus.Web.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Congressus.Web.Controllers
{
    public class CertificadosController : Controller
    {
        private readonly EventosRepository _repo = new EventosRepository();
        #region Asistentes
        //ASISTENTES//
        [Authorize(Roles = "presidente, admin")]
        public ActionResult UploadCertificadoAsistentes(CertificadoUploadVM model)
        {
            if (ModelState.IsValid)
            {
                var evento = _repo.FindById(model.Id);
                if (evento == null)
                    return HttpNotFound();

                var path = _repo.GuardarCertificados(model, "CertificadoAsistentes");
                evento.CertificadoAsistentesPath = path;
                _repo.Edit(evento);

            }
            return RedirectToAction("Administrar","Eventos", new { Id = 1 });
        }
        [Authorize(Roles = "admin,presidente")]
        public ActionResult VerCertificadoAsistentes(int id)
        {
            var evento = _repo.FindById(id);
            if (string.IsNullOrEmpty(evento.CertificadoAsistentesPath))
            {
                return HttpNotFound();
            }

            try
            {
                var renderedBytes = _repo.RenderizarCertificado(evento.Id, evento.CertificadoAsistentesPath);
                return File(renderedBytes, "application/pdf");
            }
            catch (Exception e)
            {
                ViewBag.Mensaje = e.Message;
                return View("Error");
            }
        }
        [Authorize(Roles = "asistente")]
        public ActionResult GenerarCertificadoAsistentes(int id)
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
            return RedirectToAction("Administrar","Eventos", new { id = id });
        }
        #endregion
        
        #region AsistentesXCharla
        [Authorize(Roles = "presidente, admin")]
        public ActionResult UploadCertificadoAsistentesXCharla(CertificadoUploadVM model)
        {
            if (ModelState.IsValid)
            {
                var evento = _repo.FindById(model.Id);
                if (evento == null)
                    return HttpNotFound();

                var path = _repo.GuardarCertificados(model, "CertificadoAsistentesXCharla");
                evento.CertificadoAsistentesXCharlaPath = path;
                _repo.Edit(evento);

            }
            return RedirectToAction("Administrar", "Eventos", new { Id = 1 });
        }
        public ActionResult VerCertificadoAsistenteXCharla(int id)
        {
            var evento = _repo.FindById(id);
            if (string.IsNullOrEmpty(evento.CertificadoAsistentesPath))
            {
                return HttpNotFound();
            }

            try
            {
                var renderedBytes = _repo.RenderizarCertificado(evento.Id, evento.CertificadoAsistentesXCharlaPath);
                return File(renderedBytes, "application/pdf");
            }
            catch (Exception e)
            {
                ViewBag.Mensaje = e.Message;
                return View("Error");
            }
        }
        [Authorize(Roles = "asistente")]
        public ActionResult GenerarCertificadoAsistentesXCharla(int id)
        {
            var charla = _repo.FindCharlaById(id);
            if (charla == null)
            {
                ViewBag.Mensaje = "Evento no encontrado";
                return View("Error");
            }
            var evento = charla.Evento;
            if (string.IsNullOrEmpty(evento.CertificadoAsistentesXCharlaPath))
            {
                ViewBag.Mensaje = "No se ha cargado ninguna plantilla para la generacion de certificados.";
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
                var inscripcion = charla.Inscripciones.FirstOrDefault(x => x.Asistente.UsuarioId == uid);
                if (inscripcion == null)
                {
                    ViewBag.Mensaje = "Ud no es un asistente inscripto a esta charla.";
                    return View("Error");
                }
                var nombre = inscripcion.Asistente.Nombre + " " + inscripcion.Asistente.Apellido;
                var tempPath = _repo.GenerarCertificado(evento.CertificadoAsistentesXCharlaPath, nombre, evento.Nombre, evento.FechaFin.ToShortDateString());
                var renderedBytes = _repo.RenderizarCertificado(evento.Id, tempPath);
                return File(renderedBytes, "application/pdf");
            }
            catch (Exception e)
            {
                ViewBag.Mensaje = e.Message;
                return View("Error");
            }
        }
        [Authorize(Roles = "admin, presidente")]
        public ActionResult EliminarCertificadoAsistentesXCharla(int id)
        {
            var evento = _repo.FindById(id);
            if (evento == null)
            {
                ViewBag.Mensaje = "Evento no encontrado.";
                return View("Error");
            }
            _repo.EliminarCertificadoAsistentes(evento);
            return RedirectToAction("Administrar", "Eventos", new { id = id });
        }
        #endregion

        #region Descarga
        //DESCARGA DE CERTIFICADOS//
        [Authorize(Roles = "admin, presidente")]
        public ActionResult HabilitarDescargaCertificados(int id)
        {
            var evento = _repo.FindById(id);
            if (evento == null)
            {
                ViewBag.Mensaje = "Evento no encontrado.";
                return View("Error");
            }
            _repo.HabilitarDeshabilitarCertificados(evento, true);
            return RedirectToAction("Administrar","Eventos", new { id = evento.Id });
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
            return RedirectToAction("Administrar","Eventos", new { id = evento.Id });
        }
        #endregion

        #region Oradores
        //ORADORES
        [Authorize(Roles = "admin, presidente")]
        public ActionResult UploadCertificadoOradores(CertificadoUploadVM model)
        {
            if (ModelState.IsValid)
            {
                var evento = _repo.FindById(model.Id);
                if (evento == null)
                    return HttpNotFound();

                var path = _repo.GuardarCertificados(model, "CertificadoOradores");
                evento.CertificadoCoAutores = path;
                _repo.Edit(evento);

            }
            return RedirectToAction("Administrar","Eventos", new { Id = 1 });
        }
        [Authorize(Roles = "admin,presidente")]
        public ActionResult VerCertificadoOradores(int id)
        {
            var evento = _repo.FindById(id);
            if (string.IsNullOrEmpty(evento.CertificadoCoAutores))
            {
                return HttpNotFound();
            }
            try
            {
                var renderedBytes = _repo.RenderizarCertificado(evento.Id, evento.CertificadoCoAutores);
                return File(renderedBytes, "application/pdf");
            }
            catch (Exception e)
            {
                ViewBag.Mensaje = e.Message;
                return View("Error");
            }
        }
        [Authorize(Roles = "autor")]
        public ActionResult GenerarCertificadoOradores(int id, int charlaId)
        {
            var evento = _repo.FindById(id);
            if (evento == null)
            {
                ViewBag.Mensaje = "Evento no encontrado";
                return View("Error");
            }
            if (string.IsNullOrEmpty(evento.CertificadoCoAutores))
            {
                ViewBag.Mensaje = "No se ha cargado ninguna plantilla para la generacion de certificados.";
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
                    ViewBag.Mensaje = "No se ha encontrado la charla, o esta no lo tiene a usted como orador.";
                    return View("Error");
                }
                var nombre = charla.Orador.Nombre + " " + charla.Orador.Apellido;
                var tempPath = _repo.GenerarCertificado(evento.CertificadoCoAutores, nombre, evento.Nombre, evento.FechaFin.ToShortDateString(), charla.Titulo);
                var renderedBytes = _repo.RenderizarCertificado(evento.Id, tempPath);
                return File(renderedBytes, "application/pdf");
            }
            catch (Exception e)
            {
                ViewBag.Mensaje = e.Message;
                return View("Error");
            }
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
            return RedirectToAction("Administrar","Eventos", new { id = id });
        }
        #endregion

        #region CoAutores
        //ORADORES
        [Authorize(Roles = "admin, presidente")]
        public ActionResult UploadCertificadoCoAutores(CertificadoUploadVM model)
        {
            if (ModelState.IsValid)
            {
                var evento = _repo.FindById(model.Id);
                if (evento == null)
                    return HttpNotFound();

                var path = _repo.GuardarCertificados(model, "CertificadoCoAutores");
                evento.CertificadoCoAutores = path;
                _repo.Edit(evento);

            }
            return RedirectToAction("Administrar", "Eventos", new { Id = 1 });
        }
        [Authorize(Roles = "admin,presidente")]
        public ActionResult VerCertificadoCoAutores(int id)
        {
            var evento = _repo.FindById(id);
            if (string.IsNullOrEmpty(evento.CertificadoCoAutores))
            {
                return HttpNotFound();
            }
            try
            {
                var renderedBytes = _repo.RenderizarCertificado(evento.Id, evento.CertificadoCoAutores);
                return File(renderedBytes, "application/pdf");
            }
            catch (Exception e)
            {
                ViewBag.Mensaje = e.Message;
                return View("Error");
            }
        }

        public ActionResult GenerarCertificadoCoAutores(int id)
        {
            var charla = _repo.FindCharlaById(id);
            if (charla == null)
            {
                ViewBag.Mensaje = "Charla no encontrada.";
                return View("Error");
            }
            var evento = charla.Evento;
            if (string.IsNullOrEmpty(evento.CertificadoCoAutores))
            {
                ViewBag.Mensaje = "No se ha cargado ninguna plantilla para la generacion de certificados.";
                return View("Error");
            }
            if (!evento.HabilitarDescargaCertificados)
            {
                ViewBag.Mensaje = "Los organizadores todavia no han habilitado la descarga de los certificados.";
                return View("Error");
            }
            try
            {
                var CoAutores = charla.paper.CoAutores.Replace(";", ", ");
                var tempPath = _repo.GenerarCertificado(evento.CertificadoCoAutores, null, evento.Nombre, evento.FechaFin.ToShortDateString(),null,CoAutores);
                var renderedBytes = _repo.RenderizarCertificado(evento.Id, tempPath);
                return File(renderedBytes, "application/pdf");
            }
            catch (Exception e)
            {
                ViewBag.Mensaje = e.Message;
                return View("Error");
            }
        }
        [Authorize(Roles = "admin, presidente")]
        public ActionResult EliminarCertificadoCoAutores(int id)
        {
            var evento = _repo.FindById(id);
            if (evento == null)
            {
                ViewBag.Mensaje = "Evento no encontrado.";
                return View("Error");
            }
            _repo.EliminarCertificadoCoAutores(evento);
            return RedirectToAction("Administrar", "Eventos", new { id = id });
        }
        #endregion
    }
}