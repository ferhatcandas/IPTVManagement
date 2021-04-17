using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataLayer.Repository
{
    public interface IRepository<T>
         where T : class, new()
    {
        void Insert(T entity);
        void Delete(string id);
        List<T> Get();
        List<T> Get(Expression<Func<T, bool>> predicate);
    }
}
