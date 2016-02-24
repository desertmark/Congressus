using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Congressus.Web.Models
{
    public class EvaluacionesViewModel
    {

        [Range(0, 100, ErrorMessage = "La calificacion debe ser un Numero entero entre 0 y 100")]
        public int Calificacion { get; set; }
        public string Comentario { get; set; }
        [Required(ErrorMessage = "Debe especifiarse un paperId correspondiente a un paper Existente")]
        public int paperId { get; set; }
    }
}