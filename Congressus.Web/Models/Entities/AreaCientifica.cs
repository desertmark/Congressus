using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Congressus.Web.Models.Entities
{
    [Table("AreasCientificas")]
    public class AreaCientifica
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public virtual Evento Evento { get; set; }
        public virtual MiembroComite MiembroComite { get; set; }

    }
}