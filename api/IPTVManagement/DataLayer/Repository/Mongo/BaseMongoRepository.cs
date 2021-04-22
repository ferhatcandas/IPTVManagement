using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Model;
using MongoDB.Driver;

namespace DataLayer.Repository.Mongo.Concrete
{
    public abstract class BaseMongoRepository<T> : IRepository<T>
        where T : class, IMongoEntity, new()
    {
        private readonly IMongoCollection<T> collection;
        public BaseMongoRepository(IMongoCollection<T> collection)
        {
            this.collection = collection;
        }

        public async Task DeleteAsync(string id) => await collection.DeleteOneAsync(x => x.Id == id);
        public virtual async Task<List<T>> GetAsync() => await (await collection.FindAsync(FilterDefinition<T>.Empty)).ToListAsync();
        public virtual async Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                List<T> ts = await (await collection.FindAsync(predicate)).ToListAsync();
                return ts;
            }
            catch (Exception ex)
            {

                throw ex;
            }
         
        }
        public virtual async Task<T> GetAsync(string id) => await (await collection.FindAsync(x => x.Id == id)).FirstOrDefaultAsync();
        public async Task InsertAsync(T entity) => await collection.InsertOneAsync(entity);
        public async Task UpdateAsync(T entity) => await collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
    }
}
