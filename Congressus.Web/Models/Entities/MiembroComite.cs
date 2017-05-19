using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Congressus.Web.Models.Entities
{
    [Table("MiembrosComite")]
    public class MiembroComite
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        [Display(Name = "Area Cientifica")]
        public virtual string UsuarioId { get; set; }
        public virtual ApplicationUser Usuario { get; set; }
        public virtual ICollection<AreaCientifica> AreaCientifica { get; set; }
        public virtual ICollection<Evento> Eventos { get; set; }
        public virtual ICollection<Paper> Papers { get; set; }
        public virtual ICollection<Revision> Revsiones { get; set; }

    }
}