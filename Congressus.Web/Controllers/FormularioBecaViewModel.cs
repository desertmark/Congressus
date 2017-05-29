using Congressus.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Congressus.Web.Controllers
{
    public class FormularioBecaViewModel
    {
        public int EventoId { get; set; }
        public int Id { get; set; }
        [Required]
        [Display(Name = "Nombre completo")]
        public string NombreCompleto { get; set; }
        [Required]
        public string Domicilio { get; set; }
        [Required]
        [Display(Name = "Codigo postal")]

        public int CodigoPostal { get; set; }
        #region Localidad Compelta
        [Required]
        public string Localidad { get; set; }
        [Required]
        public string Provincia { get; set; }
        [Required]
        public string Pais { get; set; }
        [Display(Name = "Localidad")]
        public string LocalidadCompleta { get { return Localidad + ", " + Provincia + ", " + Pais; } }
        #endregion

        [Required]
        public string Telefono { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Presenta trabajo en la conferencia")]
        public bool PresentaTrabajo { get; set; }
        [Required]
        [Display(Name = "Área científica")]
        public int AreaCientificaId { get; set; }
        public virtual IEnumerable<SelectListItem> AreasCientificas { get; set; }

        //Segun categoria se muestran unos campos u otros.
        [Required]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; } //Alumno de grado, Graduado, Estudiante maestría, Estudiante Doctorado
        #region Alumno de grado
        public string Universidad { get; set; }
        public string Carrera { get; set; }
        [Display(Name = "Porcentaje completado")]
        public int PorcentajeCarrera { get; set; }
        [Display(Name = "Promedio parcial")]
        public double PromedioParcial { get; set; }
        #endregion
        #region demas categorías
        [Display(Name = "Título de grado")]
        public string TituloGrado { get; set; }
        [Display(Name = "Año de graduación")]
        public int AñoGrado { get; set; }
        [Display(Name = "Título  de posgrado")]
        public string TituloPosgrado { get; set; }
        [Display(Name = "Año de posgrado")]
        public int AñoPosgrado { get; set; }
        [Display(Name = "Porcentaje de posgrado")]
        public int PorcentajePosgrado { get; set; }
        [Display(Name = "Director de posgrado")]
        public string DirectorPosgrado { get; set; }
        [Display(Name = "Lugar de trabajo de posgrado")]
        public string LugarTrabajoPosgrado { get; set; }
        [Display(Name = "Posición actual (Organizacion, empresa, etc.). Descripción:")]
        public string PosicionActual { get; set; } //Posición actual (CONICET, ANPCyT, empresa, etc.). Descripción :
        [Display(Name = "Puesto. Descripción tareas:")]
        public string Puesto { get; set; } //Puesto. Descripción tareas:
        #endregion

        public FormularioBecaViewModel(Evento evento)
        {
            EventoId = evento.Id;
            var areas = new List<SelectListItem>();
            if(evento.AreasCientificas!=null && evento.AreasCientificas.Count > 0) {
                evento.AreasCientificas.ToList().ForEach((area) => {
                    areas.Add(new SelectListItem() {
                        Value = area.Id.ToString(),
                        Text = area.Descripcion
                    });
                });
            }
            AreasCientificas = areas;
        }
    }
}