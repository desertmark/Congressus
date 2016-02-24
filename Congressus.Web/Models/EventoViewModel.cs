using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Congressus.Web.Models
{
    public class EventoViewModel
    {
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public string Lugar { get; set; }
        public string Tema { get; set; }
        public string Direccion { get; set;}
    }
}