//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using Congressus.Web.Models;
//using Congressus.Web.Models.Entities;
//using Congressus.Web.Reporting;
//using Microsoft.Reporting.WebForms;
//using System.IO;
//using Congressus.Web.Context;

//namespace Congressus.Web.Controllers
//{
//    public class CertificadosController : Controller
//    {
//        private ApplicationDbContext db = new ApplicationDbContext();

//        // GET: Certificados
//        public ActionResult Index()
//        {
//            return View(db.Certificados.ToList());
//        }

//        // GET: Certificados/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Certificado certificado = db.Certificados.Find(id);
//            if (certificado == null)
//            {
//                return HttpNotFound();
//            }
//            certificado.InsertarNombre("Fernando Gabriel Asulay");

//            LocalReport report = new LocalReport();
//            string path = Path.Combine(Server.MapPath("/Reporting"), "Certificado2.rdl");

//            string cert = System.IO.File.ReadAllText(path);
//            cert = cert.Replace("[Nombre]", "Fernando Gabriel Asulay");
//            string tempPath = Path.Combine(Path.GetTempPath(), "Certificado.rdl");
//            System.IO.File.WriteAllText(tempPath, cert);

//            report.ReportPath = tempPath;
//            ReportDataSource rd = new ReportDataSource("CertificadoDS", new List<Certificado>() {certificado});            
//            report.DataSources.Add(rd);
//            report.EnableExternalImages = true;
//            string reportType = "PDF";
//            string mimeType;
//            string encoding;
//            string fileNameExtension;

//            string deviceInfo =

//            "<DeviceInfo>" +
//            "  <OutputFormat>" + id + "</OutputFormat>" +
//            "  <PageWidth>11in</PageWidth>" +
//            "  <PageHeight>8.5in</PageHeight>" +
//            "  <MarginTop>0.25in</MarginTop>" +
//            "  <MarginLeft>0.25in</MarginLeft>" +
//            "  <MarginRight>0.25in</MarginRight>" +
//            "  <MarginBottom>0.25in</MarginBottom>" +
//            "</DeviceInfo>";

//            Warning[] warnings;
//            string[] streams;
//            byte[] renderedBytes;

//            renderedBytes = report.Render(
//                reportType,
//                deviceInfo,
//                out mimeType,
//                out encoding,
//                out fileNameExtension,
//                out streams,
//                out warnings);
//            return File(renderedBytes, "application/pdf");
//        }

//        // GET: Certificados/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: Certificados/Create
//        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
//        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "Id,LogoPath,TextoCuerpo,TextoFecha,TextoFirma1,TextoFirma2,TextoFirma3")] Certificado certificado)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Certificados.Add(certificado);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(certificado);
//        }

//        // GET: Certificados/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Certificado certificado = db.Certificados.Find(id);
//            if (certificado == null)
//            {
//                return HttpNotFound();
//            }
//            return View(certificado);
//        }

//        // POST: Certificados/Edit/5
//        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
//        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "Id,LogoPath,TextoCuerpo,TextoFecha,TextoFirma1,TextoFirma2,TextoFirma3")] Certificado certificado)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(certificado).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(certificado);
//        }

//        // GET: Certificados/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Certificado certificado = db.Certificados.Find(id);
//            if (certificado == null)
//            {
//                return HttpNotFound();
//            }
//            return View(certificado);
//        }

//        // POST: Certificados/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            Certificado certificado = db.Certificados.Find(id);
//            db.Certificados.Remove(certificado);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
