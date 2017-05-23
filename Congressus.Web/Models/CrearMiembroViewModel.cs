using Congressus.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Congressus.Web.Models
{
    public class CrearMiembroViewModel
    {
        public string Nombre { get; set; }
        
        public string Apellido { get; set; }

        [Display(Name = "Área científica")]
        public int AreaCientificaId { get; set; }

        public IEnumerable<SelectListItem> AreasCientificas { get; set; }

        [Required]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string Confirm { get; set; }

        public int EventoId { get; set; }
        public IEnumerable<SelectListItem> Eventos { get; set; }

        public CrearMiembroViewModel()
        {
        }

        public CrearMiembroViewModel(IEnumerable<Evento> eventos, CrearMiembroViewModel model = null)
        {
            //Areas del primer evento si el evento existe
            List<SelectListItem> areas = new List<SelectListItem>();
            if (eventos != null)
            {
                eventos.FirstOrDefault()?.AreasCientificas?.ToList().ForEach((area) =>
                {
                    areas.Add(new SelectListItem()
                    {
                        Text = area.Descripcion,
                        Value = area.Id.ToString()
                    });
                });
            }

            Eventos = new SelectList(eventos, "Id", "Nombre");
            AreasCientificas = areas;
            if (model != null)
            {
                Nombre = model.Nombre;
                Apellido = model.Apellido;
                Email = model.Email;
                EventoId = model.EventoId;
                AreaCientificaId = model.AreaCientificaId;
            }


        }
    }

}