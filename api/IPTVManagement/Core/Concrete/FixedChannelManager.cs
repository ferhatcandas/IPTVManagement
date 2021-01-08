using Core.Abstract;
using DataLayer;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Concrete
{
    public class FixedChannelManager : IFixedChannelService
    {
        private readonly ChannelRepository channelRepository;

        public FixedChannelManager(ChannelRepository channelRepository)
        {
            this.channelRepository = channelRepository;
        }


        public List<Channel> Get() => channelRepository.Get();
    }
}
