using Congressus.Web.Attributes;
using Congressus.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Congressus.Web.Models
{
    public class EventoViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        [IsDateBefore("FechaFin", ErrorMessage = "La fecha límite de presentación de los trabajos debe ser anterior a la fecha de finalización del evento.")]
        [Display(Name = "Fecha límite presentación de trabajos")]
        public DateTime FechaFinTrabajos { get; set; }

        [IsDateBefore("FechaFin", ErrorMessage = "La fecha de límite de inscripción debe ser anterior a la fecha de finalización del evento.")]
        [Display(Name = "Fecha límite de inscripción al evento")]
        public DateTime FechaFinInscripcion { get; set; }

        [IsDateBefore("FechaFin", ErrorMessage = "La fecha de inicio debe ser anterior a la fecha de finalización del evento.")]
        [Display(Name = "Fecha de inicio")]
        public DateTime FechaInicio { get; set; }

        [IsDateAfter("FechaInicio", ErrorMessage = "La fecha de fin debe ser posterior a la fecha de inicio.")]
        [Display(Name = "Fecha de finalización")]
        public DateTime FechaFin { get; set; }
        public string Lugar { get; set; }
        public string Tema { get; set; }
        [Display(Name = "Email de Contacto")]
        public string EmailContacto { get; set; }
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }
        [Display(Name ="Áreas científicas")]
        public string AreasCientificas { get; set; }

        public EventoViewModel() { }
        public EventoViewModel(Evento evento)
        {
            Nombre = evento.Nombre;
            FechaFinInscripcion = evento.FechaFinInscripcion;
            FechaFinTrabajos = evento.FechaFinTrabajos;
            FechaInicio = evento.FechaInicio;
            FechaFin = evento.FechaFin;
            Lugar = evento.Lugar;
            Direccion = evento.Direccion;
            EmailContacto = evento.EmailContacto;
            Tema = evento.Tema;
            AreasCientificas = "";
            if (evento.AreasCientificas != null && evento.AreasCientificas.Count>0) { 
                foreach (var area in evento.AreasCientificas)
                    AreasCientificas += area.Descripcion + ";";
                AreasCientificas = AreasCientificas.Remove(AreasCientificas.Length - 1);
            }

        }

        public Evento ToEvento(Evento evento = null)
        {
            //Si el evento es null --> crear areas segun lo que contiene el VM pertenecientes al este evento.
            if (evento == null)
            {
                evento = new Evento();
                if (!string.IsNullOrEmpty(AreasCientificas))
                {
                    evento.AreasCientificas = new List<AreaCientifica>();

                    Array.ForEach(AreasCientificas.Split(';'), (area) =>
                    {
                        evento.AreasCientificas.Add(new AreaCientifica()
                        {
                            Evento = evento,
                            Descripcion = area,
                        });
                    });
                }
            } else
            {   //si el evento no null --> leer las areas del evento si no estan en el VM borrarlas. 
                //Luego leer las areas del VM si no estan en el evento agregarlas
                var areas = new List<AreaCientifica>(evento.AreasCientificas);//se copia la coleccion pq será modificada durante el bucle.
                if (string.IsNullOrEmpty(AreasCientificas))
                    evento.AreasCientificas.Clear();
                else { 
                    foreach (var area in areas)
                    {
                        if (!AreasCientificas.Split(';').Contains(area.Descripcion))
                            evento.AreasCientificas.Remove(area);
                    }
                    foreach (var area in AreasCientificas.Split(';'))
                    {
                        if (!evento.AreasCientificas.Any(a => a.Descripcion == area))
                            evento.AreasCientificas.Add(new AreaCientifica() {
                                Evento = evento,
                                Descripcion = area
                            });
                    }
                }
            }
            
            evento.Nombre = Nombre;
            evento.FechaFinInscripcion = FechaFinInscripcion;
            evento.FechaFinTrabajos = FechaFinTrabajos;
            evento.FechaInicio = FechaInicio;
            evento.FechaFin = FechaFin;
            evento.Lugar = Lugar;
            evento.EmailContacto = EmailContacto;
            evento.Direccion = Direccion;
            evento.Tema = Tema;
            return evento;
        }
    }
}