using Congressus.Web.Models.Entities;
using System.ComponentModel.DataAnnotations;
namespace Congressus.Web.Models
{
    public class MiembroViewModel
    {
        public int Id { get; set; }
        [MinLength(3)]
        public string Nombre { get; set; }
        [MinLength(2)]
        public string Apellido { get; set; }
        public MiembroViewModel()
        {

        }
        public MiembroViewModel(MiembroComite miembro)
        {
            Id = miembro.Id;
            Nombre = miembro.Nombre;
            Apellido = miembro.Apellido;        
        }
    }
}