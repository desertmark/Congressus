﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Congressus.Web.Models.Entities
{
    [Table("Asistentes")]
    public class Asistente
    {
        public int Id { get; set; }
        public int DNI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public virtual string UsuarioId { get; set; }
        public virtual ApplicationUser Usuario { get; set; }
        public virtual ICollection<InscripcionEvento> Eventos { get; set; }
        public virtual ICollection<InscripcionCharla> Charlas { get; set; }
    }
}