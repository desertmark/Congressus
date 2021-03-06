﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name ="Fecha limite presentacion de trabajos")]
        public DateTime FechaFinTrabajos {get; set;}
        [Display(Name = "Fecha limite de inscripcion al evento")]
        public DateTime FechaFinInscripcion { get; set; }
        [Display(Name = "Fecha de inicio")]
        public DateTime FechaInicio { get; set; }
        [Display(Name = "Fecha de finalizacion")]
        public DateTime FechaFin { get; set; }
        public string Lugar { get; set; }
        public string Tema { get; set; }        
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
        public string TextoBienvenida { get; set; }
        [Display(Name = "Areas Cientifica")]
        public string AreasCientificas { get; set; }
        public virtual ICollection<Paper> Papers{ get; set; }        
        public virtual ICollection<MiembroComite> Comite { get; set; }
        public virtual ICollection<Charla> Charlas { get; set; }
        public virtual ICollection<InscripcionEvento> Inscripciones { get; set; }
        public virtual ICollection<SeccionEvento> Secciones { get; set; }


        
    }
}