using Congressus.Web.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Web;

namespace Congressus.Web.Models.Entities
{
    [Table("Paper")]
    public class Paper : ObjetoConArchivo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        [Display(Name = "Area Cientifica")]
        public string AreaCientifica { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Fecha { get; set; }
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }
        //public string Path { get; set; }
        //[NotMapped]
        //public byte[] Archivo { get; set; }
        public virtual Evento Evento { get; set; }
        public virtual MiembroComite Evaluador { get; set; }
        public virtual Autor Autor { get; set; }
        public string CoAutores { get; set; }
        public virtual ICollection<Revision> Revisiones { get; set; }
        public virtual Evaluacion Evaluacion { get; set; }
        public virtual string Estado { get; set; } = "Sin aceptar o rechazar";


        public override string GetRelativeFileDirectory()
        {
            if (Autor != null) {
                return "/Content/Files/" + Autor.Usuario.Email + "/Papers/" + Id;
            }
            return null;
        }

        /// <summary>
        /// Se debe ejecutar ApplicationDbContext.SaveChanges() luego de ejecutar este metodo para que los cambios surtan efectos en la base de datos.
        /// </summary>
        public void DeletePaper(ApplicationDbContext db)
        {
            BorrarArchivo();//borra el archivo del paper
            //borrar hijos
            db.Revisiones.RemoveRange(Revisiones);
            if (Evaluacion != null) 
                db.Evaluacions.Remove(Evaluacion);
            db.Papers.Remove(this);
        }

        //public Paper()
        //{
        //    if(!string.IsNullOrEmpty(Path))
        //    {
        //        Archivo = File.ReadAllBytes(Path);                
        //    }
        //}
        //public string GetPaperDirectory()
        //{
        //    var server = HttpContext.Current.Server;
        //    var paperPath = server.MapPath("/content/papers");
        //    var userPath = paperPath + "\\" + Autor.Usuario.Email;

        //    return userPath;
        //}
        //public string LinkDescarga()
        //{
        //    var link = "/content/papers/"+ Autor.Usuario.Email +"/"+ NombreArchivo();
        //    return link;
        //}

        //public void GuardarPaper(HttpPostedFileBase Archivo)
        //{

        //    var server = HttpContext.Current.Server;
        //    var paperPath = server.MapPath("../content/papers");
        //    var userPath = paperPath +"/" + Autor.Usuario.Email;
        //    var userPath = GetPaperDirectory();
        //    if(!Directory.Exists(userPath))
        //    {
        //        Directory.CreateDirectory(userPath);
        //    }
        //    Path = userPath + "\\" + Archivo.FileName;

        //    Transformar httpPostedFile a Byte[]
        //    MemoryStream stream = new MemoryStream();
        //    Archivo.InputStream.CopyTo(stream);
        //    this.Archivo = stream.ToArray();

        //    File.WriteAllBytes(Path, this.Archivo);

        //}

        //public void BorrarPaper()
        //{
        //    var userPath = GetPaperDirectory();
        //    var path = userPath + "\\" + NombreArchivo();
        //    if (File.Exists(path))
        //    {                                
        //        File.Delete(path);
        //    }
        //}

        //public string NombreArchivo()
        //{
        //    string[] nombre = Path.Split('\\');
        //    return nombre[nombre.Length - 1];
        //}


    }

}