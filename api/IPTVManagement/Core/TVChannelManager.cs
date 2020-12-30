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
            var existChannel = existChannels.FirstOrDefault(x => x.Stream.Contains(channel.Stream));
            if (existChannels == null)
            {
                return (false, "channel already exists with name " + existChannel.Name);
            }
            else
            {
                channelRepository.Insert(channel);
                return (true, "channel saved");
            }
        }
        public void SaveChannels(List<TVChannel> tVChannels) => channelRepository.Save(tVChannels);

        public List<TVChannel> GetChannels() => channelRepository.Get();

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

        internal TVChannel GetChannel(string channelId) => GetChannels().FirstOrDefault(x => x.Id == channelId);

        internal void UpdateStatus(string channelId)
        {
            var channel = GetChannel(channelId);
            channel.IsActive = !channel.IsActive;
            UpdateChannel(channelId, channel);
        }
    }
}
