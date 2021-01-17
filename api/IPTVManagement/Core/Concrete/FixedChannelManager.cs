using Core.Abstract;
using DataLayer;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace Core.Concrete
{
    public class FixedChannelManager : IFixedChannelService
    {
        private readonly ChannelRepository channelRepository;

        public FixedChannelManager(ChannelRepository channelRepository)
        {
            this.channelRepository = channelRepository;
        }

        public void Add(Channel channel)
        {
            if (!channelRepository.Exist(channel.Name))
            {
                channelRepository.Insert(channel);
            }
        }

        public void Delete(string channelId)
        {
            channelRepository.Delete(channelId);
        }

        public List<CommonChannelModel> Get()
        {
            List<Channel> listChannel = channelRepository.Get();
            return listChannel.Select(x => x.ToCommonChannel()).ToList();
        }

        public CommonChannelModel Get(string channelId)
        {
            return channelRepository.FirstOrDefault(channelId);
        }

        public void UpdateChannel(string channelId, Channel channel)
        {
            channelRepository.UpdateChannel(channelId, channel);
        }

        public void UpdateStatus(string channelId)
        {
            channelRepository.UpdateStatus(channelId);
        }
    }
}
