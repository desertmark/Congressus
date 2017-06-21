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
        [Display(Name = "Código postal")]

        public int CodigoPostal { get; set; }
        #region Localidad Compelta
        [Required]
        public string Localidad { get; set; }
        [Required]
        public string Provincia { get; set; }
        [Required]
        [Display(Name = "País")]
        public string Pais { get; set; }
        [Display(Name = "Localidad")]
        public string LocalidadCompleta { get { return Localidad + ", " + Provincia + ", " + Pais; } }
        #endregion

        [Required]
        [Display(Name = "Teléfono")]
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

        public FormularioBecaViewModel(){}

        public FormularioBecaViewModel(Evento evento)
        {
            SetearSelectLists(evento);
        }       

        public FormularioBecaViewModel(FormularioBeca beca)
        {
            NombreCompleto = beca.NombreCompleto;
            AreaCientificaId = beca.AreaCientificaId;
            AñoGrado = beca.AñoGrado;
            AñoPosgrado = beca.AñoPosgrado;
            Carrera = beca.Carrera;
            CategoriaId = beca.CategoriaId;
            CodigoPostal = beca.CodigoPostal;
            Domicilio = beca.Domicilio;
            DirectorPosgrado = beca.DirectorPosgrado;
            Email = beca.Email;
            EventoId = beca.EventoId;
            Localidad = beca.Localidad;
            LugarTrabajoPosgrado = beca.LugarTrabajoPosgrado;
            Pais = beca.Pais;
            PorcentajeCarrera = beca.PorcentajeCarrera;
            PorcentajePosgrado = beca.PorcentajePosgrado;
            PosicionActual = beca.PosicionActual;
            PresentaTrabajo = beca.PresentaTrabajo;
            PromedioParcial = beca.PromedioParcial;
            Provincia = beca.Provincia;
            Puesto = beca.Puesto;
            Telefono = beca.Telefono;
            TituloGrado = beca.TituloGrado;
            TituloPosgrado = beca.TituloPosgrado;
            Universidad = beca.Universidad;
            Id = beca.Id;

            SetearSelectLists(beca.Evento);
        }

        public void SetearSelectLists(Evento evento)
        {
            EventoId = evento.Id;
            var areas = new List<SelectListItem>();
            areas.Add(new SelectListItem()
            {
                Text = "Seleccione un área científica",
                Value = "0"
            });
            if (evento.AreasCientificas != null && evento.AreasCientificas.Count > 0)
            {
                evento.AreasCientificas.ToList().ForEach((area) => {
                    areas.Add(new SelectListItem()
                    {
                        Value = area.Id.ToString(),
                        Text = area.Descripcion
                    });
                });
            }
            AreasCientificas = areas;
        }

        public FormularioBeca ToFormularioBeca(FormularioBecaViewModel model)
        {
            return new FormularioBeca()
            {
                NombreCompleto = model.NombreCompleto,
                AreaCientificaId = model.AreaCientificaId,
                AñoGrado = model.AñoGrado,
                AñoPosgrado = model.AñoPosgrado,
                Carrera = model.Carrera,
                CategoriaId = model.CategoriaId,
                CodigoPostal = model.CodigoPostal,
                Domicilio = model.Domicilio,
                DirectorPosgrado = model.DirectorPosgrado,
                Email = model.Email,
                EventoId = model.EventoId,
                Localidad = model.Localidad,
                LugarTrabajoPosgrado = model.LugarTrabajoPosgrado,
                Pais = model.Pais,
                PorcentajeCarrera = model.PorcentajeCarrera,
                PorcentajePosgrado = model.PorcentajePosgrado,
                PosicionActual = model.PosicionActual,
                PresentaTrabajo = model.PresentaTrabajo,
                PromedioParcial = model.PromedioParcial,
                Provincia = model.Provincia,
                Puesto = model.Puesto,
                Telefono = model.Telefono,
                TituloGrado = model.TituloGrado,
                TituloPosgrado = model.TituloPosgrado,
                Universidad = model.Universidad,
                Id = model.Id,

            };
        }
    }
}