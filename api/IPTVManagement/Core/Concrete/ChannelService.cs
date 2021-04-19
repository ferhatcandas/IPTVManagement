using Core.Abstract;
using DataLayer.Repository.Mongo;
using Model;
using System.Collections.Generic;
using System.IO;
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
            await channelMongoRepository.DeleteAsync(channelId);
        }

        public async Task<CommonChannelModel> GetChannel(string channelId)
        {
            return await channelMongoRepository.GetAsync(channelId);
        }

        public async Task<List<CommonChannelModel>> GetChannels()
        {
            return await channelMongoRepository.GetAsync();
        }

        public async Task<Stream> GetStream()
        {
            //var channels = GetChannels();
            //var bytes = streamManager.Export(channels);

            return null;
        }

        public async Task UpdateChannel(string channelId, Channel request)
        {
            await channelMongoRepository.UpdateAsync(request.ToCommonChannel());
        }

        public async Task UpdateStatus(string channelId)
        {
            var channel = await GetChannel(channelId);
            channel.IsActive = !channel.IsActive;
            await channelMongoRepository.UpdateAsync(channel);
        }
    }
}
