using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Congressus.Web.Models.Entities
{
    public class RevisionViewModel
    {
        public int Id { get; set; }
        public string Estado { get; set; }
        public string Comentario { get; set; }
        public string Path { get; set; }
        public int PaperId { get; set; }
        public virtual HttpPostedFileBase Archivo{ get; set; }
    }
}