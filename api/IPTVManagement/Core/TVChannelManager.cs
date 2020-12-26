using DataLayer;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public class TVChannelManager
    {
        private readonly TVChannelRepository channelRepository;

        public TVChannelManager(TVChannelRepository channelRepository)
        {
            this.channelRepository = channelRepository;
        }
        public (bool status, string message) SaveChannel(TVChannel channel)
        {
            var existChannels = GetChannels();
            var existChannel = existChannels.FirstOrDefault(x => x.StreamLink.Contains(channel.StreamLink));
            if (existChannels == null)
            {
                return (false, "channel already exists with name " + existChannel.ShowChannelName);
            }
            else
            {
                channelRepository.Insert(channel);
                return (true, "channel saved");
            }
        }
        public void SaveChannels(List<TVChannel> tVChannels)
        {
            channelRepository.Save(tVChannels);
        }

        public List<TVChannel> GetChannels()
        {
            return channelRepository.Get();
        }

        public (bool status, string message) UpdateChannel(M3U8Channel channel)
        {
            var existChannels = GetChannels();
            var existChannel = existChannels.FirstOrDefault(x => x.StreamLink.Contains(channel.StreamLink));
            if (existChannels != null && existChannel.StreamLink != channel.StreamLink)
            {
                existChannel.StreamLink = channel.StreamLink;
                channelRepository.Save(existChannels);
                return (true, "channel updated");
            }
            return (false, "channel already update");
        }

        internal void RemoveChannels(List<string> ids)
        {
            var existChannels = GetChannels().Where(x => !ids.Contains(x.Id));

            SaveChannels(existChannels.ToList());
        }

        internal void UpdateChannel(string channelId, TVChannel channel)
        {
            var existChannels = GetChannels();

            var existChannel = existChannels.FirstOrDefault(x => x.Id == channelId);
            if (existChannel != null)
            {
                existChannels = existChannels.Where(x => x.Id != channelId).ToList();
                channel.Id = channelId;
                existChannels.Add(channel);
                SaveChannels(existChannels);
            }
        }

        internal TVChannel GetChannel(string channelId)
        {
            var existChannels = GetChannels();

            return existChannels.FirstOrDefault(x => x.Id == channelId);
        }
    }
}
