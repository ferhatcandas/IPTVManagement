using Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            List<M3U8Channel> channels = integrationManager.FetchChannels();

            List<TVChannel> existChannels = channelManager.GetChannels().Where(x => x.IsActive).ToList();
            existChannels.AddRange(channels.Where(x => x.HasIntegration).Select(x => x.ToTVChannel(true, x.Integration)));
            List<M3U8Channel> filteredChannels = new List<M3U8Channel>();

            foreach (M3U8Channel channel in channels)
            {
                foreach (TVChannel existChannel in existChannels.Where(x => !x.Found))
                {
                    foreach (string tag in existChannel.Tags)
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
            List<M3U8Channel> activeChannels = M3UManager.ControlAndGetActiveChannels(filteredChannels);

            List<M3U8Channel> newChannels = new List<M3U8Channel>();

            foreach (M3U8Channel channel in activeChannels)
            {
                foreach (TVChannel existChannel in existChannels.Where(x => !x.Found))
                {
                    foreach (string tag in existChannel.Tags)
                    {
                        if (channel.ChannelName.ToLower().Contains(tag.ToLower()))
                        {
                            M3U8Channel newChannel = existChannel.ToM3U();
                            newChannel.ChannelName = channel.ChannelName;
                            newChannel.StreamLink = channel.StreamLink;
                            newChannels.Add(newChannel);
                        }
                    }
                }
            }

            foreach (TVChannel existChannel in existChannels.Where(x => x.Found))
            {
                newChannels.Add(existChannel.ToM3U());
            }

            return M3UManager.Export(newChannels);


        }

        public void UpdateStatus(string channelId)
        {
            channelManager.UpdateStatus(channelId);

        }

        public Stream GetStream()
        {
            byte[] bytes = FetchList();
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        public List<TVChannel> GetTVChannels(bool addional = false)
        {
            List<TVChannel> list = channelManager.GetChannels();
            if (addional)
            {
                list.AddRange(integrationManager.GetAddionalList());
            }
            return list;
        }

        public TVChannel GetTVChannel(string channelId)
        {
            return channelManager.GetChannel(channelId);
        }

        public void AddChannel(TVChannel channel)
        {
            channelManager.SaveChannel(channel);
        }

        public void RemoveChannel(string channelId)
        {
            channelManager.RemoveChannels(new List<string> { channelId });
        }

        public void UpdateChannel(string channelId, TVChannel channel)
        {
            channelManager.UpdateChannel(channelId, channel);
        }
    }
}
