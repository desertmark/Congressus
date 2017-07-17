using Congressus.Web.Controllers;
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
        public AreaCientifica FindAreaById(int AreaId)
        {
            return _db.AreasCientificas.FirstOrDefault(a => a.Id == AreaId);
        }
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
        public Charla FindCharlaById(int id)
        {
            return _db.Charlas.FirstOrDefault(x => x.Id == id);
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
            foreach (var a in evento.AreasCientificas)
            {
                a.MiembroComite = presidente;
                _db.AreasCientificas.Add(a);
            }
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
            /*borrar dependencias*/
            //borrar papers
            evento.Papers.ToList().ForEach(p => p.DeletePaper(_db));
            _db.Papers.RemoveRange(evento.Papers);
            //borrar areas
            _db.AreasCientificas.RemoveRange(evento.AreasCientificas);
            //Desvincular miembros
            evento.Comite.Clear();
            //borrar secciones
            _db.Secciones.RemoveRange(evento.Secciones);
            evento.Secciones.Clear();
            //Borar el evento
            _db.Eventos.Remove(evento);
            _db.SaveChanges();
        }
        public string GuardarCertificados(CertificadoUploadVM model, string FileName)
        {
            var server = HttpContext.Current.Server;

            var carpetaPath = "/Content/Files/Eventos/" + model.Id + "/Certificados/";
            var fileNamePath = carpetaPath + FileName + "." + model.Certificado.FileName.Split('.').Last();

            if (!Directory.Exists(server.MapPath(carpetaPath)))
                Directory.CreateDirectory(server.MapPath(carpetaPath));

            model.Certificado.SaveAs(server.MapPath(fileNamePath));
            return fileNamePath;
        }

        public byte[] RenderizarCertificado(int eventoId, string path)
        {

            if (!path.Contains(":\\"))
                path = HttpContext.Current.Server.MapPath(path);

            LocalReport report = new LocalReport();
            
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
            "  <OutputFormat>" + eventoId + "</OutputFormat>" +
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

        public string GenerarCertificado(string path, string nombre, string evento, string fecha, string charla = null, string CoAutores = null)
        {

            var server = HttpContext.Current.Server;
            var rdlText = File.ReadAllLines(server.MapPath(path));
            if (rdlText.Any(x => x.Contains("[Nombre]")))
            {
                var value = rdlText.First(x => x.Contains("[Nombre]"));
                var index = Array.IndexOf(rdlText, value);
                value = value.Replace("[Nombre]", nombre);
                rdlText.SetValue(value, index);
            }
            if (rdlText.Any(x => x.Contains("[Evento]")))
            {
                var value = rdlText.First(x => x.Contains("[Evento]"));
                var index = Array.IndexOf(rdlText, value);
                value = value.Replace("[Evento]", evento);
                rdlText.SetValue(value, index);
            }

            if (rdlText.Any(x => x.Contains("[Fecha]")))
            {
                var value = rdlText.First(x => x.Contains("[Fecha]"));
                var index = Array.IndexOf(rdlText, value);
                value = value.Replace("[Fecha]", fecha);
                rdlText.SetValue(value, index);
            }
            if(rdlText.Any(x=>x.Contains("[Charla]")) && charla != null)
            {
                var value = rdlText.First(x => x.Contains("[Charla]"));
                var index = Array.IndexOf(rdlText, value);
                value = value.Replace("[Charla]", charla);
                rdlText.SetValue(value, index);
            }
            if (rdlText.Any(x => x.Contains("[CoAutores]")))
            {
                var value = rdlText.First(x => x.Contains("[CoAutores]"));
                var index = Array.IndexOf(rdlText, value);
                value = value.Replace("[CoAutores]", CoAutores);
                rdlText.SetValue(value, index);
            }
            var tempPath = Path.GetTempFileName();
            File.WriteAllLines(tempPath, rdlText);
            return tempPath;
        }

        public void EliminarCertificadoAsistentes(Evento evento)
        {
            if (File.Exists(evento.CertificadoAsistentesPath))
                File.Delete(evento.CertificadoAsistentesPath);
            evento.CertificadoAsistentesPath = "";
            Edit(evento);
        }
        public void EliminarCertificadoAsistentesXCharla(Evento evento)
        {
            if (File.Exists(evento.CertificadoAsistentesXCharlaPath))
                File.Delete(evento.CertificadoAsistentesXCharlaPath);
            evento.CertificadoAsistentesXCharlaPath = "";
            Edit(evento);
        }
        public void EliminarCertificadoOradores(Evento evento)
        {
            if (File.Exists(evento.CertificadoCoAutores))
                File.Delete(evento.CertificadoCoAutores);
            evento.CertificadoCoAutores = "";
            Edit(evento);
        }
        public void EliminarCertificadoCoAutores(Evento evento)
        {
            if (File.Exists(evento.CertificadoCoAutores))
                File.Delete(evento.CertificadoCoAutores);
            evento.CertificadoCoAutores = "";
            Edit(evento);
        }
        public void HabilitarDeshabilitarCertificados(Evento evento, bool accion)
        {
            evento.HabilitarDescargaCertificados = accion;
            Edit(evento);
        }

        public List<string> GuardarImagenes(ImagenesUploadVM model,string carpeta ="")
        {
            var Server = HttpContext.Current.Server;
            var path = "/Content/Files/Eventos/Images/" + model.Id + "/" + carpeta + "/";

            if (!Directory.Exists(Server.MapPath(path)))
                Directory.CreateDirectory(Server.MapPath(path));
            var imagenes = new List<string>();

            foreach (var imagen in model.Imagenes)
            {
                var filePath = path + imagen.FileName;
                imagen.SaveAs(Server.MapPath(filePath));
                imagenes.Add(filePath);
            }
            return imagenes;
        }

        public bool EliminarImagenes(int eventoId, string carpeta)
        {
            var Server = HttpContext.Current.Server;
            var path = "/Content/Files/Eventos/Images/" + eventoId + "/" + carpeta + "/";
            if (!Directory.Exists(path))
                return true;
            try { 
                Directory.Delete(path, true);
                return true;
            }catch(Exception e)
            {
                return false;
            }
        }

        public List<string> GuardarArchivos(List<HttpPostedFileBase> archivos, string relPath)
        {
            var Server = HttpContext.Current.Server;
            var absolutePath = Server.MapPath(relPath);
            if (!Directory.Exists(absolutePath))
                Directory.CreateDirectory(absolutePath);

            var relPaths = new List<string>();
            foreach (var archivo in archivos)
            {
                var filePath = relPath + archivo.FileName;
                archivo.SaveAs(Server.MapPath(filePath));
                relPaths.Add(filePath);
            }
            return relPaths;
        }

        public bool AsignarAreaAMiembro(AreaCientifiaViewModel model)
        {
            var area = FindAreaById(model.Id);
            var nuevoMiembro = FindMimembroById(model.MiembroComiteId);

            if (nuevoMiembro == null)
                return false;

            area.MiembroComite = nuevoMiembro;
            _db.Entry(area).State = EntityState.Modified;
            _db.SaveChanges();

            return true;
        }
    }
}