using DataLayer.Repository.Mongo.Abstract;
using Model.Integration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Mongo.Concrete
{
    public class IntegrationRepository<T> : BaseMongoRepository<IntegrationBase<T>>, IIntegrationRepository<T>
        where T : IntegrationSettings
    {
        public IntegrationRepository(IMongoCollection<IntegrationBase<T>> collection) : base(collection)
        {
        }
    }
}
