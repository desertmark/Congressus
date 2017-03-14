using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Congressus.Web.Controllers
{
    public class ImagenesInicioVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        //[MaxLength (5)]
        public List<HttpPostedFileBase> Imagenes { get; set; }
    }
}