using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Congressus.Web.Models.Entities
{
    public class InscripcionEvento
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public virtual Asistente Asistente { get; set; }
        public virtual Evento Evento { get; set; }
    }
}