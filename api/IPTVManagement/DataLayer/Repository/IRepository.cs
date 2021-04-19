using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IRepository<T>
         where T : class, new()
    {
        Task InsertAsync(T entity);
        Task DeleteAsync(string id);
        Task UpdateAsync(T entity);
        Task<List<T>> GetAsync();
        Task<T> GetAsync(string id);
        Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate);
    }
}
