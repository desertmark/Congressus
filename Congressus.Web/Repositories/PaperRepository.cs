using Congressus.Web.Models;
using Congressus.Web.Models.Entities;
using Congressus.Web.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Congressus.Web.Repositories
{
    public class PaperRepository : Repository<Paper>
    {
        /// <summary>
        /// Prepara un nuevo ViewModel para la creacion de un nuevo paper, validando que el evento exista y su fecha de presentacion de trabajos no este vencida.
        /// En caso de error el metodo devuelve nulo y el mensaje de error correspondiente se captura en mensajeError.
        /// </summary>
        /// <param name="EventoId">Id del evento al cual se envia el paper</param>
        /// <param name="mensajeError">variable de salida para mostrar el error en caso que ocurra.</param>
        /// <returns></returns>
        public PaperViewModel CrearPaperVm(int EventoId, out string mensajeError)
        {
            var evento = _db.Eventos.FirstOrDefault(x => x.Id == EventoId);
            if (evento == null)
            { 
                mensajeError = "Evento no econtrado.";
                return null;
            }
            if (evento.FechaFinTrabajos.Date < DateTime.Today.Date)
            {
                mensajeError = "Ya ha finalizado la fecha de presentacion de trabajos para este evento.";
                return null;
            }
            mensajeError = null;
            return GetNewPaperVm(evento);
        }

        public PaperViewModel GetNewPaperVm(Evento Evento)
        {
            var model = new PaperViewModel()
            {
                EventoId = Evento.Id,
                AreasCientifica = new SelectList(Evento.AreasCientificas,"Id","Descripcion"),
                Fecha = DateTime.Today
            };
            return model;
        }

        public bool CrearPaper(PaperViewModel model, string UserId)
        {
            var autor = _db.Autores.FirstOrDefault(a => a.UsuarioId == UserId);
            model.Autor = autor;
            var paper = MapFromVm(model);
            //Asignacion automatica del paper al evaluador del area correspondiente.
            var evaluador = paper.Evento.Comite.FirstOrDefault(x => x.AreaCientifica == paper.AreaCientifica);
            if (evaluador != null)
                paper.Evaluador = evaluador;
            //Guardar model.Archivo
            Add(paper);
            return true;
        }
        public Paper MapFromVm(PaperViewModel model)
        {
            var evento = _db.Eventos.Find(model.EventoId);
            var paper = new Paper()
            {
                Nombre = model.Nombre,
                AreaCientifica = evento.AreasCientificas.FirstOrDefault( a => a.Id == model.AreaCientificaId),
                Descripcion = model.Descripcion,
                Fecha = model.Fecha,
                Path = model.Path,
                Autor = model.Autor,
                Evento = evento,
                CoAutores = model.CoAutores
            };
            return paper;
        }
    }
}