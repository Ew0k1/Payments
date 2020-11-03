using System;
using System.Collections.Generic;

namespace Payments.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T Find(int id);
        IEnumerable<T> Find(Func<T, bool> predicate);
        IEnumerable<T> FindAll();
        void Create(T item);
        void Update(T item);
        void Delete(int id);
    }
}
