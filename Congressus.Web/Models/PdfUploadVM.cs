using Congressus.Web.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Congressus.Web.Models
{
    public class PdfUploadVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [FileExtension(new string[] { "pdf" })]
        public List<HttpPostedFileBase> Documentos { get; set; }
    }
}