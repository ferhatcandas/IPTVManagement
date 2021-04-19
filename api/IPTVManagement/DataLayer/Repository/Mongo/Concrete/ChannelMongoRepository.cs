using DataLayer.Repository.Mongo.Abstract;
using DataLayer.Repository.Mongo.Concrete;
using Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataLayer.Repository.Mongo
{
    public class ChannelMongoRepository : BaseMongoRepository<CommonChannelModel>, IChannelRepository
    {
        public ChannelMongoRepository(IMongoCollection<CommonChannelModel> collection) : base(collection)
        {
        }
    }
}
