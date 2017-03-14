using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Congressus.Web.Controllers
{
    public class TextoInicioVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Texto{ get; set; }
    }
}