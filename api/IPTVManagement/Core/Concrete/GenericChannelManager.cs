using Core.Abstract;
using Core.Utils;
using DataLayer;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Concrete
{
    public class GenericChannelManager : IGenericChannelService
    {
        private readonly GenericChannelRepository channelRepository;

        public GenericChannelManager(GenericChannelRepository channelRepository)
        {
            this.channelRepository = channelRepository;
        }
        public List<CommonChannelModel> Get()
        {
            List<CommonChannelModel> channels = new List<CommonChannelModel>();
            var genericSettings = channelRepository.Get();

            foreach (var integration in genericSettings)
            {
                channels.AddRange(integration.GetCommonChannels());
            }

            return channels;
        }
    }
}
