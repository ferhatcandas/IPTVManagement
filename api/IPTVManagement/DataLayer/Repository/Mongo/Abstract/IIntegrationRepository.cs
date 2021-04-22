using Model.Integration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Mongo.Abstract
{
    public interface IIntegrationRepository<T> : IRepository<IntegrationBase<T>>
          where T : IntegrationSettings
    {
    }
}
