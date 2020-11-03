using Payments.DAL.EF;
using Payments.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Payments.DAL.Repositories
{
    public class EFRepository<T> : IRepository<T> where T : class, State
    {
        protected ApplicationContext db;

        public EFRepository(ApplicationContext context)
        {
            db = context;
        }

        public void Create(T item)
        {
            db.Set<T>().Add(item);
        }

        public void Delete(int id)
        {
            var item = db.Set<T>().Find(id);
            if (item != null)
                db.Set<T>().Remove(item);
        }

        public T Find(int id)
        {
            return db.Set<T>().Find(id);
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return db.Set<T>().Where(predicate).ToList();
        }

        public IEnumerable<T> FindAll()
        {
            return db.Set<T>();
        }

        public void Update(T item)
        {
            db.Entry<T>(item).State = EntityState.Modified;
        }

        public void SoftDelete(int id)
        {
            var item = db.Set<T>().Find(id);
            if (item != null)
            {
                item.IsDeleted = true;
            }
            Update(item);
        }

        public void Restore(int id)
        {
            var item = db.Set<T>().Find(id);
            if (item != null)
            {
                item.IsDeleted = false;
            }
            Update(item);
        }

        public void Block(int id)
        {
            var item = db.Set<T>().Find(id);
            if (item != null)
            {
                item.IsBlocked = true;
            }
            Update(item);
        }

        public void Unblock(int id)
        {
            var item = db.Set<T>().Find(id);
            if (item != null)
            {
                item.IsBlocked = false;
            }
            Update(item);
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}
