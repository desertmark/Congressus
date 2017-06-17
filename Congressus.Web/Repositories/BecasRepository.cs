using Congressus.Web.Controllers;
using Congressus.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Congressus.Web.Repositories
{
    public class BecasRepository : Repository<FormularioBeca>
    {
        public IEnumerable<FormularioBeca> GetByEventoId(int eventoId)
        {
            return _db.FormulariosBecas
                .Include(form => form.AreaCientifica)
                .Include(form=>form.Evento)
                .Where(form => form.EventoId == eventoId);
        }

        public override FormularioBeca FindById(int id)
        {
            return _db.FormulariosBecas
                .Include(form => form.AreaCientifica)
                .Include(form => form.Evento)
                .FirstOrDefault(form => form.Id == id);
        }

        public bool Add(FormularioBecaViewModel model)
        {
            var formBeca = model.ToFormularioBeca(model);
            try
            {
                Add(formBeca);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
    }
}