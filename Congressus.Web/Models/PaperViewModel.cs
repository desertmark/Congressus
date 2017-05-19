using Congressus.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Congressus.Web.Models
{
    public class PaperViewModel
    {
        public int Id { get; set; }
        public int EventoId { get; set; }
        public string Nombre { get; set; }
        public int AreaCientificaId { get; set; }
        [Display(Name = "Area Cientifica")]
        public IEnumerable<SelectListItem> AreasCientifica { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Fecha { get; set; }
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }
        public string Path { get; set; }
        [NotMapped]
        public HttpPostedFileBase Archivo { get; set; }
        public virtual Autor Autor { get; set; }
        [Display(Name ="Co-Autores")]
        public string CoAutores { get; set; }
    }
}