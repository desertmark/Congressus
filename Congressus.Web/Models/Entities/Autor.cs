using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace Congressus.Web.Models.Entities
{
    [Table("Autores")]
    public class Autor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public virtual string UsuarioId { get; set; }
        public virtual ApplicationUser Usuario { get; set; }
        public virtual ICollection<Paper> Papers { get; set; }
    }
}
