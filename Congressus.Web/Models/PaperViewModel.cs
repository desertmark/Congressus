using Congressus.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Congressus.Web.Models
{
    public class PaperViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        [Display(Name = "Area Cientifica")]
        public string AreaCientifica { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Fecha { get; set; }
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }
        public string Path { get; set; }
        [NotMapped]
        public HttpPostedFileBase Archivo { get; set; }
        public virtual Autor Autor { get; set; }

    }
}