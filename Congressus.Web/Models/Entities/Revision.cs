using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Congressus.Web.Models.Entities
{
    [Table("Revisiones")]
    public class Revision : ObjetoConArchivo
    {
        public int Id { get; set; }
        public string Estado { get; set; }
        public string Comentario { get; set; }
        public virtual Paper Paper { get; set; }
        public virtual MiembroComite MiembroComite { get; set; }

        public static IEnumerable EstadosPosibles()
        {
            var estados = new List<string> { "Corregir", "Probable Aceptacion", "Rechazado" };
            return estados;
        }

        public override string GetRelativeFileDirectory()
        {
            return "/Content/Files/" + Paper.Autor.Usuario.Email + "/Papers/" + Paper.Id + "/Revisiones"; 
        }
    }
}