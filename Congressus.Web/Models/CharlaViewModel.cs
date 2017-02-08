using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Congressus.Web.Models
{
    public class CharlaViewModel
    {
        public int Id { get; set; }
        [Required]
        public int EventoId { get; set; }
        [Required]
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        [Display(Name = "Fecha y hora")]
        [Required]        
        public DateTime Fecha { get; set; }
        public int Cupo { get; set; }
        public string Lugar { get; set; }
        public SelectList Papers { get; set; }
        public int PaperId { get; set; }
    }
}