using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Congressus.Web.Models.Entities
{
    [Table("Evaluaciones")]
    public class Evaluacion
    {
        [ForeignKey("Paper")]
        public int Id { get; set; }
        public int Calificacion { get; set; }
        public string Comentario { get; set; }

        public virtual MiembroComite MiembroComite { get; set; }
        public virtual Paper Paper { get; set; }
    }
}