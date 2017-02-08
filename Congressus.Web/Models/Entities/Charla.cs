using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Congressus.Web.Models.Entities
{
    [Table("Charlas")]
    public class Charla
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public int Cupo { get; set; }
        public DateTime FechaHora { get; set; }
        public string Lugar { get; set; }
        public virtual Evento Evento { get; set; }
        public virtual Autor Orador { get; set; }
        public virtual Paper paper { get; set; }
        public virtual ICollection<InscripcionCharla> Inscripciones{ get; set; }

    }
}