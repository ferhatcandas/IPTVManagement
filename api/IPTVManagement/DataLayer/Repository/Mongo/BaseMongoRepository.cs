using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Model;
using MongoDB.Driver;

namespace DataLayer.Repository.Mongo
{
    public abstract class BaseMongoRepository<T> : IRepository<T>
        where T : class, IMongoEntity, new()
    {
        private readonly IMongoCollection<T> collection;
        public BaseMongoRepository(IMongoCollection<T> collection)
        {
            this.collection = collection;
        }

        public void Delete(string id) => collection.DeleteOne(x => x.Id == id);

        public List<T> Get() => collection.Find(null).ToList();

        public List<T> Get(Expression<Func<T, bool>> predicate) => collection.Find(predicate).ToList();

        public void Insert(T entity) => collection.InsertOne(entity);

    }
}
