using Congressus.Web.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Congressus.Web.Models
{
    public class CertificadoUploadVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [FileExtension(new string[] { "rdl","rdlc" })]
        public HttpPostedFileBase Certificado{ get; set; }
    }
}