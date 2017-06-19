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
        [AllowHtml]
        public string TextoBienvenida{ get; set; }
    }
}