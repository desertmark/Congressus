using Congressus.Web.Models;
using Congressus.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Congressus.Web.Repositories
{
    public class CharlasRepository : Repository<Charla>
    {
        public CharlaViewModel GetCharlaViewModel(int eventoId)
        {
            var evento = _db.Eventos.Find(eventoId);
            var vm = new CharlaViewModel()
            {
                Cupo = 0,
                Fecha = DateTime.Now,
                EventoId = eventoId,
                Papers = new SelectList(evento.Papers.Where(p => p.Estado == "Aceptado"), "Id", "Nombre"),
            };
            return vm;
        }
        public CharlaViewModel GetCharlaViewModel(Charla charla)
        {
            var vm = new CharlaViewModel()
            {
                Cupo = charla.Cupo,
                Fecha = charla.FechaHora,
                EventoId = charla.Evento.Id,
                Papers = new SelectList(charla.Evento.Papers.Where(p => p.Estado == "Aceptado"), "Id", "Nombre"),
                Titulo = charla.Titulo,
                Descripcion = charla.Descripcion,
                Lugar = charla.Lugar,
                PaperId = charla.paper.Id
            };
            return vm;
        }
        public Charla GetCharlaFromVm(CharlaViewModel model)
        {
            var evento = _db.Eventos.Find(model.EventoId);
            var paper = _db.Papers.Find(model.PaperId);
            var charla = new Charla()
            {
                Id = model.Id,
                Evento = evento,
                paper = paper,
                Titulo = model.Titulo,
                Descripcion = model.Descripcion,
                FechaHora = model.Fecha,
                Lugar = model.Lugar,
                Tipo = model.Cupo>0 ? "Taller" : "Charla",
                Cupo = model.Cupo,
                Orador = paper.Autor
            };

            return charla;
        }
    }
}