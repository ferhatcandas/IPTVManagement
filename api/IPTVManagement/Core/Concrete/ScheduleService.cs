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
        private readonly GenericChannelRepository genericChannelRepository;

        public ScheduleService(
            IEnumerable<IIntegration> integrations,
            IChannelRepository channelRepository,
            ChannelRepository fileChannelRepository,
            GenericChannelRepository genericChannelRepository)
        {
            this.integrations = integrations;
            this.channelRepository = channelRepository;
            this.fileChannelRepository = fileChannelRepository;
            this.genericChannelRepository = genericChannelRepository;
        }
        public async Task Syncronize()
        {
            foreach (var item in integrations)
                await Process(await item.GetAsync());
        }

        public async Task TransferChannels()
        {
            var channels = fileChannelRepository.Get().Select(x => x.ToCommonChannel()).ToList();
            await Process(channels);
        }

        public async Task TransferIntegrations()
        {
            var generics = genericChannelRepository.Get();
            foreach (var item in generics)
            {
                switch (item.IntegrationType)
                {
                    case "Fixed":
                        break;
                    case "Half":
                        break;
                    case "Full":
                        break;
                    default:
                        break;
                }
            }


            throw new NotImplementedException();
        }




        private async Task Process(List<CommonChannelModel> channels)
        {
            foreach (var item in channels)
                await channelRepository.InsertAsync(item);
        }
    }
}
