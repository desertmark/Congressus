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


    }

}