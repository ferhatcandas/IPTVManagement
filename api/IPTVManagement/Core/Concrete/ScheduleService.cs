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
            List<Task> tasks = new List<Task>();
            tasks.Add(new Task(() =>
            {
                var existChannels = channelRepository.GetAsync(x => x.Integration == "Fixed").Result;
                Control(existChannels);
                foreach (var item in existChannels)
                {
                    channelRepository.UpdateAsync(item).Wait();
                }
            }));
            foreach (var item in integrations)
            {
                tasks.Add(new Task(() =>
                {
                    Proc(item);
                }));
            }



            tasks.ForEach(x => x.Start());
            await Task.WhenAll(tasks);
        }

        private void Proc(IIntegration item)
        {
            var existChannels = channelRepository.GetAsync(x => x.Integration == item.GetType().Name).Result;
            foreach (var channel in existChannels)
                channelRepository.DeleteAsync(channel.Id).Wait();
            Process(item.GetAsync().Result).Wait();
        }

        public async Task TransferChannels()
        {
            var channels = fileChannelRepository.Get().Select(x => x.ToCommonChannel()).ToList();
            await Process(channels);
        }

        private void Control(List<CommonChannelModel> channels)
        {
            ChannelController channelController = new ChannelController();
            channelController.ControlAndGet(channels);
        }
        private async Task Process(List<CommonChannelModel> channels)
        {
            Control(channels);
            foreach (var item in channels.Where(x => x.HasStream))
                await channelRepository.InsertAsync(item);
        }
    }
}
