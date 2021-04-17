using Core.Abstract;
using DataLayer.Repository.Mongo;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Concrete
{
    public class ChannelService : IChannelService
    {
        private readonly ChannelMongoRepository channelMongoRepository;
        public ChannelService(ChannelMongoRepository channelMongoRepository)
        {
            this.channelMongoRepository = channelMongoRepository;
        }
        public async Task DeleteChannel(string channelId)
        {
            channelMongoRepository.Delete(channelId);
        }

        public async Task<CommonChannelModel> GetChannel(string channelId)
        {
            return channelMongoRepository.Get(x => x.Id == channelId).FirstOrDefault();
        }

        public async Task<List<CommonChannelModel>> GetChannels()
        {
            return channelMongoRepository.Get();
        }

        public async Task<Stream> GetStream()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateChannel(string channelId, Channel request)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateStatus(string channelId)
        {
            throw new NotImplementedException();
        }
    }
}
