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

        public Manager(M3UManager M3UManager, TVChannelManager channelManager)
        {
            this.M3UManager = M3UManager;
            this.channelManager = channelManager;
        }



        public void FetchList()
        {
            var channels = M3UManager.FetchChannels();

            var existChannels = channelManager.GetChannels().Where(x => x.IsActive).ToList();

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

            M3UManager.Export(newChannels);


        }

        public List<M3U8Channel> GetSources() => M3UManager.FetchChannels();

        public List<M3U8Channel> DownloadAndGet(string v) => M3UManager.DownloadAndGet(v);

        public void RemoveFromList(List<string> ids) => channelManager.RemoveChannels(ids);

        public void SaveChannels(List<TVChannel> channels) => channelManager.SaveChannels(channels);

        public (bool status, string message) SaveChannel(M3U8Channel existData) => channelManager.SaveChannel(existData.ToTVChannel());


        public void ChangeStatus(List<string> ids) => channelManager.ChangeStatus(ids);

        public (bool status, string message) UpdateChannel(M3U8Channel existData) => channelManager.UpdateChannel(existData);
        public List<TVChannel> GetTVChannels() => channelManager.GetChannels();
        public TVChannelModel GetTVChannel(string channelId) => channelManager.GetChannel(channelId).ToTVChannelModel();
        public void AddChannel(TVChannel channel) => channelManager.SaveChannel(channel);
        public void RemoveChannel(string channelId) => channelManager.RemoveChannels(new List<string> { channelId });
        public void UpdateChannel(string channelId, TVChannel channel) => channelManager.UpdateChannel(channelId,channel);
    }
}
