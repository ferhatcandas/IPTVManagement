using Core.Abstract;
using Core.Integrations.Abstract;
using DataLayer;
using DataLayer.Repository.Mongo.Abstract;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Concrete
{
    public class ScheduleService : IScheduleService
    {
        private readonly IEnumerable<IIntegration> integrations;
        private readonly IChannelRepository channelRepository;
        private readonly ChannelRepository fileChannelRepository;

        public ScheduleService(
            IEnumerable<IIntegration> integrations,
            IChannelRepository channelRepository,
            ChannelRepository fileChannelRepository)
        {
            this.integrations = integrations;
            this.channelRepository = channelRepository;
            this.fileChannelRepository = fileChannelRepository;
        }
        public async Task Syncronize()
        {
            foreach (var item in integrations)
                await Process(await item.GetAsync());
        }

        public async Task TransferChannels()
        {
            var channels = fileChannelRepository.Get().Select(x=>x.ToCommonChannel()).ToList();
            await Process(channels);
        }

        private async Task Process(List<CommonChannelModel> channels)
        {
            foreach (var item in channels)
                await channelRepository.InsertAsync(item);
        }
    }
}
