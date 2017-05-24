using Congressus.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Congressus.Web.Models
{
    public class AreaCientifiaViewModel
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int EventoId { get; set; }
        public int MiembroComiteId { get; set; }
        public IEnumerable<SelectListItem> Miembros { get; set; }

        public AreaCientifiaViewModel()
        {
        }
        public AreaCientifiaViewModel(AreaCientifica area)
        {
            Id = area.Id;
            Descripcion = area.Descripcion;
            EventoId = area.Evento.Id;
            MiembroComiteId = area.MiembroComite != null ? area.MiembroComite.Id : 0;
            Miembros = new SelectList(area.Evento.Comite, "Id", "NombreCompleto");
        }
    }
}