using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Congressus.Web.Models
{
    public class AgregarMiembroViewModel
    {
        public int miembroId { get; set; }
        public int eventoId { get; set; }
    }

    public class AsignarMiembroViewModel
    {
        public int miembroId { get; set; }
        public int paperId { get; set; }
    }
}