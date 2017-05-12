using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Congressus.Web.Controllers
{
    public class TextoInicioVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Texto{ get; set; }
    }
}