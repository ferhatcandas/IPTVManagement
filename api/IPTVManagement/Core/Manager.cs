using DataLayer;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace Core
{
    public class Manager
    {
        private readonly M3UManager M3UManager;
        private readonly TVChannelManager channelManager;
        private readonly IntegrationManager integrationManager;
        public Manager(M3UManager M3UManager, TVChannelManager channelManager, IntegrationManager integrationManager)
        {
            this.M3UManager = M3UManager;
            this.channelManager = channelManager;
            this.integrationManager = integrationManager;
        }

        public byte[] FetchList()
        {
            var channels = integrationManager.FetchChannels();

            var existChannels = channelManager.GetChannels().Where(x => x.IsActive).ToList();
            existChannels.AddRange(channels.Where(x => x.IsAddList).Select(x => x.ToTVChannel()));
            List<M3U8Channel> filteredChannels = new List<M3U8Channel>();

            foreach (var channel in channels)
            {
                foreach (var existChannel in existChannels.Where(x => !x.Found))
                {
                    foreach (var tag in existChannel.ChannelTags)
                    {
                        if (channel.ChannelName.ToLower().Contains(tag.ToLower()))
                        {
                            if (!filteredChannels.Any(x => x.StreamLink == channel.StreamLink))
                            {
                                filteredChannels.Add(channel);
                            }
                        }
                    }
                }
            }
            var activeChannels = M3UManager.ControlAndGetActiveChannels(filteredChannels);

            List<M3U8Channel> newChannels = new List<M3U8Channel>();

            foreach (var channel in activeChannels)
            {
                foreach (var existChannel in existChannels.Where(x => !x.Found))
                {
                    foreach (var tag in existChannel.ChannelTags)
                    {
                        if (channel.ChannelName.Contains(tag))
                        {
                            var newChannel = existChannel.ToM3U();
                            newChannel.ChannelName = channel.ChannelName;
                            newChannel.StreamLink = channel.StreamLink;
                            newChannels.Add(newChannel);
                        }
                    }
                }
            }

            foreach (var existChannel in existChannels.Where(x => x.Found))
            {
                newChannels.Add(existChannel.ToM3U());
            }

            return M3UManager.Export(newChannels);


        }

        public void UpdateStatus(string channelId)
        {
            var channels = GetTVChannels();
            var channel = channels.FirstOrDefault(x => x.Id == channelId);
            channel.IsActive = !channel.IsActive;
            channelManager.SaveChannels(channels);
        }

        public Stream GetStream()
        {
            var bytes = FetchList();
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        public List<TVChannel> GetTVChannels()
        {
            var list =  channelManager.GetChannels();
            list.AddRange(integrationManager.GetAddionalList());

            return list;
        }

        public TVChannelModel GetTVChannel(string channelId) => channelManager.GetChannel(channelId).ToTVChannelModel();
        public void AddChannel(TVChannel channel) => channelManager.SaveChannel(channel);
        public void RemoveChannel(string channelId) => channelManager.RemoveChannels(new List<string> { channelId });
        public void UpdateChannel(string channelId, TVChannel channel) => channelManager.UpdateChannel(channelId, channel);
    }
}
