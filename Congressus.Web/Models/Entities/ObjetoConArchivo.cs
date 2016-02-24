using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Congressus.Web.Models.Entities
{
    public abstract class ObjetoConArchivo
    {
        public string Path { get; set; }
        [NotMapped]
        private byte[] archivo;
        [NotMapped]
        public virtual byte[] Archivo
        {
            get
            {
                if(archivo == null)
                {
                    archivo = LeerArchivo();
                }
                return archivo;
            }
            set { archivo = value; }
        }

        private byte[] LeerArchivo()
        {
            var path = GetAbsolutFileDirectory() + "/" + NombreArchivo();
            return File.ReadAllBytes(path);            
        }
        public abstract string GetRelativeFileDirectory();
        public string GetAbsolutFileDirectory()
        {
            var server = HttpContext.Current.Server;
            return  server.MapPath(GetRelativeFileDirectory());
        }
        public string LinkDescarga()
        {
            Type type = GetType();            
            var link = GetRelativeFileDirectory() +"/" + NombreArchivo();
            return link;
        }

        public void GuardarArchivo(HttpPostedFileBase Archivo)
        {
            var userPath = GetAbsolutFileDirectory();
            if (!Directory.Exists(userPath))
            {
                Directory.CreateDirectory(userPath);
            }
            var now = DateTime.Now.ToFileTime();

            var SplitName = Archivo.FileName.Split('.');
            var NoExtensionName = string.Join(".", SplitName.Except(new string[] { SplitName.Last() }).ToArray());
            var extension = SplitName.Last();

            Path = userPath + "\\" + NoExtensionName + "_" + now + "." + extension;

            //Transformar httpPostedFile a Byte[]
            MemoryStream stream = new MemoryStream();
            Archivo.InputStream.CopyTo(stream);
            this.Archivo = stream.ToArray();

            File.WriteAllBytes(Path, this.Archivo);

        }

        public void BorrarArchivo()
        {
            var userPath = GetAbsolutFileDirectory();
            var path = userPath + "\\" + NombreArchivo();
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public string NombreArchivo()
        {
            if(Path != null) { 
                string[] nombre = Path.Split('\\');
                return nombre[nombre.Length - 1]; // o usar el metodo .Last()
            }
            return null;
        }

    }
}