using Congressus.Web.Models;
using Congressus.Web.Models.Entities;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;

namespace Congressus.Web.Repositories
{
    public class EventosRepository : Repository<Evento>
    {
        public IQueryable<Evento> FindByPattern(string patron)
        {       
            return _db.Eventos.Where(e => e.Nombre.Contains(patron) ||
                                           e.Lugar.Contains(patron) ||
                                           e.Direccion.Contains(patron) ||
                                           e.Tema.Contains(patron));             
        }

        public IEnumerable<Evento> FindByMiembroUserId(string UserId)
        {
            return _db.Miembros.Single(m => m.UsuarioId == UserId).Eventos;
        }

        public IEnumerable<Evento> FindByAutorUserId(string UserId)
        {
            var autor = _db.Autores.Single(a => a.UsuarioId == UserId); // buscar autor para ese Id de usuario 
            return autor.Papers.Select(p => p.Evento).Distinct(); // buscar en la lista de papers de ese autor todos los eventos en que participa habiendo enviado un paper
        }

        public IEnumerable<MiembroComite> MiembrosDeTodosLosEventos(string UserId)
        {
            var eventos = FindByMiembroUserId(UserId);
            var miembros = new List<MiembroComite>();
            foreach (var e in eventos)
            {
                miembros = miembros.Concat(e.Comite.ToList()).ToList();
            }
            return miembros.Distinct();
        }
        public MiembroComite FindMimembroById(int id)
        {
            return _db.Miembros.FirstOrDefault(x=> x.Id == id);
        }

        public void AgregarMiembroComite(int miembroId, int eventoId)
        {
            var miembro = FindMimembroById(miembroId);
            var evento = FindById(eventoId);

            if (miembro.Eventos == null)
                miembro.Eventos = new List<Evento>() { evento };
            else
                miembro.Eventos.Add(evento);

            evento.Comite.Add(miembro);

            _db.SaveChanges();
        }

        public void RetirarMiembroComite(int miembroId, int eventoId)
        {
            var miembro = FindMimembroById(miembroId);
            var evento = FindById(eventoId);

            evento.Comite.Remove(miembro);

            _db.Entry(evento).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void CrearEventoByOrganizador(Evento evento, string userId)
        {
            var presidente = _db.Miembros.Single(p => p.Usuario.Id == userId);
            evento.Presidente = presidente;
            evento.Presidente.Eventos.Add(evento);
            Add(evento);
        }
        public void CrearEventoByAdmin(Evento evento, string userId)
        {
            var miembro = _db.Miembros.FirstOrDefault(m => m.UsuarioId == userId);
            //Si hay un miembro con el usuario de este admin lo usa de presidente, sino crea un nuevo miembro para que sea presidente y lo asocia con el id de usuario del admin.
            evento.Presidente = (miembro != null) ? miembro : new MiembroComite() { UsuarioId = userId };
            evento.Presidente.Eventos.Add(evento);
            Add(evento);
        }

        public void EliminarEvento(int id)
        {
            Evento evento = FindById(id);

            evento.Papers.ToList().ForEach(p => p.DeletePaper(_db));
            _db.Papers.RemoveRange(evento.Papers);
            evento.Comite.Clear();
            _db.Eventos.Remove(evento);
            _db.SaveChanges();
        }
        public void GuardarCertificadoAsistentes(int eventoId, string path, HttpPostedFileBase certificado)
        {
            var evento = FindById(eventoId);
            evento.CertificadoAsistentesPath = path;
            GuardarCertificado(evento, path, certificado);
        }
        public void GuardarCertificadoOradores(int eventoId, string path, HttpPostedFileBase certificado)
        {
            var evento = FindById(eventoId);
            evento.CertificadoOradoresPath = path;
            GuardarCertificado(evento, path, certificado);
        }
        private void GuardarCertificado(Evento evento, string path, HttpPostedFileBase certificado)
        {
            MemoryStream stream = new MemoryStream();
            certificado.InputStream.CopyTo(stream);
            var bytes = stream.ToArray();

            File.WriteAllBytes(path, bytes);

            Edit(evento);
        }

        public byte[] RenderizarCertificado(Evento evento)
        {
            
            LocalReport report = new LocalReport();
            string path = evento.CertificadoAsistentesPath;
            if (!File.Exists(path))
                throw new FileNotFoundException();

            report.ReportPath = path;
            report.EnableExternalImages = true;
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + evento.Id + "</OutputFormat>" +
            "  <PageWidth>11in</PageWidth>" +
            "  <PageHeight>8.5in</PageHeight>" +
            "  <MarginTop>0.25in</MarginTop>" +
            "  <MarginLeft>0.25in</MarginLeft>" +
            "  <MarginRight>0.25in</MarginRight>" +
            "  <MarginBottom>0.25in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = report.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return renderedBytes;
        }
        public void EliminarCertificadoAsistentes(Evento evento)
        {
            if (File.Exists(evento.CertificadoAsistentesPath))
                File.Delete(evento.CertificadoAsistentesPath);
            evento.CertificadoAsistentesPath = "";
            Edit(evento);
        }

        public void EliminarCertificadoOradores(Evento evento)
        {
            if (File.Exists(evento.CertificadoOradoresPath))
                File.Delete(evento.CertificadoOradoresPath);
            evento.CertificadoOradoresPath = "";
            Edit(evento);
        }
    }
}