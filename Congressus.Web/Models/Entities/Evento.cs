using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Congressus.Web.Models.Entities
{
    [Table("Eventos")]
    public class Evento
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public string Lugar { get; set; }
        public string Tema { get; set; }        
        public string Direccion { get; set; }
        public virtual MiembroComite Presidente { get; set; }
        public virtual ICollection<Paper> Papers{ get; set; }        
        public virtual ICollection<MiembroComite> Comite { get; set; }
    }
}