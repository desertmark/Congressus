using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Congressus.Web.Models.Entities
{
    [Table("SeccionesEvento")]
    public class SeccionEvento
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Titulo { get; set; }
        public string Cuerpo { get; set; }
        public virtual Evento Evento { get; set; }
    }
}