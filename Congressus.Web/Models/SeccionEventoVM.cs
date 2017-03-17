using Congressus.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Congressus.Web.Models
{
    public class SeccionEventoVM
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Titulo { get; set; }
        [UIHint("tinymce_jquery_full"),AllowHtml]
        public string Cuerpo { get; set; }
        [Required]
        public int EventoId { get; set; }
    }
}