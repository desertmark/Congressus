using Congressus.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Congressus.Web.Repositories
{
    public class AsistentesRepository : Repository<Asistente>
    {

        public Asistente FindByUserId(string userId)
        {
            return _db.Asistentes.Single(x => x.UsuarioId == userId);
        }
        public Evento FindEventoById(int id)
        {
            return _db.Eventos.FirstOrDefault(x => x.Id == id);
        }
        public Charla FindCharlaById(int id)
        {
            return _db.Charlas.FirstOrDefault(x => x.Id == id);
        }
        public void InscribirEvento(Asistente asistente, Evento evento)
        {
            var inscripcion = new InscripcionEvento()
            {
                Fecha = DateTime.Now,
                Asistente = asistente,
                Evento = evento,
            };
            _db.InscripcionesEvento.Add(inscripcion);
            _db.SaveChanges();
        }
        public void InscribirCharla(Asistente asistente, Charla charla)
        {
            var inscripcion = new InscripcionCharla()
            {
                Asistente = asistente,
                Charla = charla,
                Fecha = DateTime.Now,
            };
            _db.InscripcionesCharla.Add(inscripcion);
            _db.SaveChanges();
        } 
        public InscripcionEvento BuscarInscripcionEvento(int id)
        {
            return _db.InscripcionesEvento.FirstOrDefault(x => x.Id == id);
        }
        public void EliminarInscripcionEvento(InscripcionEvento inscripcion)
        {            
            
            var charlas = _db.InscripcionesCharla.Where(x => x.Asistente.Id == inscripcion.Asistente.Id && 
                                                        x.Charla.Evento.Id == inscripcion.Evento.Id
                                                        );//Eliminar las inscripciones de este asistente a charlas de este evento.
            _db.InscripcionesCharla.RemoveRange(charlas);
            _db.InscripcionesEvento.Remove(inscripcion);//Eliminar inscripcion al evento
            _db.SaveChanges();
        }
        public InscripcionCharla BuscarInscripcionCharla(int id)
        {
            return _db.InscripcionesCharla.FirstOrDefault(x => x.Id == id);
        }
        public void EliminarInscripcionCharla(InscripcionCharla inscripcion)
        {
            _db.InscripcionesCharla.Remove(inscripcion);
            _db.SaveChanges();
        }

    }
}