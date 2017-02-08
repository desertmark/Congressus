using Congressus.Web.Context;
using Congressus.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Congressus.Web.Repositories
{
    public abstract class Repository<T> where T:class
    {
        protected ApplicationDbContext _db = new ApplicationDbContext();
        
        public IEnumerable<T> GetAll()
        {
            return _db.Set<T>().ToList();
        }

        public T FindById(int id)
        {
            return _db.Set<T>().Find(id);
        }

        public void Add(T entity)
        {
            _db.Set<T>().Add(entity);
            _db.SaveChanges();
        }
        public void Edit(T entity)
        {
            _db.Entry<T>(entity).State = EntityState.Modified;
            _db.SaveChanges();
        }
        public void Delete(int id)
        {
            var entity = FindById(id);
            _db.Set<T>().Remove(entity);
            _db.SaveChanges();
        }
    }
}