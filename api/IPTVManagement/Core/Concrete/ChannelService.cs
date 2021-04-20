using Core.Abstract;
using DataLayer.Repository.Mongo;
using DataLayer.Repository.Mongo.Abstract;
using Model;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Core.Concrete
{
    public class ChannelService : IChannelService
    {
        private readonly IChannelRepository channelMongoRepository;
        private readonly StreamManager streamManager;

        public ChannelService(IChannelRepository channelMongoRepository, StreamManager streamManager)
        {
            this.channelMongoRepository = channelMongoRepository;
            this.streamManager = streamManager;
        }
        public async Task DeleteChannel(string channelId) => await channelMongoRepository.DeleteAsync(channelId);

        public async Task<CommonChannelModel> GetChannel(string channelId) => await channelMongoRepository.GetAsync(channelId);

        public async Task<List<CommonChannelModel>> GetChannels() => await channelMongoRepository.GetAsync();

        public async Task<Stream> GetStream() => new MemoryStream(await streamManager.Export(await GetChannels()));

        public async Task UpdateChannel(string channelId, Channel request) => await channelMongoRepository.UpdateAsync(request.ToCommonChannel());

        public async Task UpdateStatus(string channelId)
        {
            var channel = await GetChannel(channelId);
            channel.IsActive = !channel.IsActive;
            await channelMongoRepository.UpdateAsync(channel);
        }
    }
}
