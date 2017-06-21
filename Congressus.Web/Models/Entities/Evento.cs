using Congressus.Web.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Congressus.Web.Models.Entities
{
    [Table("Eventos")]
    public class Evento
    {
        public int Id { get; set; }
        public string Nombre { get; set; }


        [Display(Name = "Fecha límite presentación de trabajos")]
        public DateTime FechaFinTrabajos {get; set;}


        [Display(Name = "Fecha límite de inscripción al evento")]
        public DateTime FechaFinInscripcion { get; set; }


        [Display(Name = "Fecha de inicio")]
        public DateTime FechaInicio { get; set; }


        [IsDateAfter("FechaInicio", ErrorMessage ="La fecha de fin debe ser posterior a la fecha de inicio.")]
        [Display(Name = "Fecha de finalización")]
        public DateTime FechaFin { get; set; }
        public string Lugar { get; set; }
        public string Tema { get; set; }
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }
        public bool HabilitarDescargaCertificados { get; set; }
        public virtual MiembroComite Presidente { get; set; }
        public string CertificadoAsistentesPath { get; set; }
        public string CertificadoOradores { get; set; }
        public string CertificadoCoAutores { get; set; }
        public string CertificadoAsistentesXCharlaPath { get; set; }
        public string LogoPath { get; set; }
        public string ProgramaPath { get; set; }
        public string ImagenesInicio { get; set; }
        public string ImagenesSponsors { get; set; }
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string TextoBienvenida { get; set; }

        [Display(Name = "Áreas Científica")]
        public virtual ICollection<AreaCientifica> AreasCientificas { get; set; }
        public virtual ICollection<Paper> Papers{ get; set; }        
        public virtual ICollection<MiembroComite> Comite { get; set; }
        public virtual ICollection<Charla> Charlas { get; set; }
        public virtual ICollection<InscripcionEvento> Inscripciones { get; set; }
        public virtual ICollection<SeccionEvento> Secciones { get; set; }
        public virtual ICollection<FormularioBeca> Becas { get; set; }

        
    }
}