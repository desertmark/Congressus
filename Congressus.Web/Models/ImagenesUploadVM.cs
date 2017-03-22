using Congressus.Web.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Congressus.Web.Controllers
{
    public class ImagenesUploadVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [FileExtension(new string[] { ".jpg", ".jpeg", ".png" })]
        public List<HttpPostedFileBase> Imagenes { get; set; }
    }
}