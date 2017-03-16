using Congressus.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Congressus.Web.Models;

namespace Congressus.Web.Repositories
{
    public class SeccionesRepository : Repository<SeccionEvento>
    {
        public Evento FindEventoById(int id)
        {
            return _db.Eventos.FirstOrDefault(x => x.Id == id);
        }

        public SeccionEvento GetSeccionFromVM(SeccionEventoVM model, Evento evento = null)
        {
            if (evento == null)
                evento = FindEventoById(model.EventoId);
            if(evento == null)
                return null;
            return new SeccionEvento()
            {
                Id = model.Id,
                Nombre = model.Nombre,
                Titulo = model.Titulo,
                Cuerpo = model.Cuerpo,
                Evento = evento
            };

        }
    }
}