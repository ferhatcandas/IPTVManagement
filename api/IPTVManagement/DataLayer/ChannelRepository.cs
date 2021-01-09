using DataLayer.Repository;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer
{
    public class ChannelRepository : FileRepository<Channel>
    {
        public ChannelRepository() : base("channels")
        {
        }


        public void Delete(string channelId)
        {
            var channels = Get();
            channels = channels.Where(x => x.Id != channelId).ToList();
            Save(channels);
        }

        public void UpdateChannel(string channelId, Channel channel)
        {
            var channels = Get();
            channels = channels.Where(x => x.Id != channelId).ToList();
            channel.Id = channelId;
            channels.Add(channel);
            Save(channels);
        }

        public bool Exist(string name)
        {
            return Get().Any(x => x.Name.ToLower() == name.Trim().ToLower());
        }

        public void UpdateStatus(string channelId)
        {
            var channels = Get();
            var existChannel = channels.FirstOrDefault(x => x.Id == channelId);
            existChannel.IsActive = !existChannel.IsActive;
            Save(channels);
        }

        public CommonChannelModel FirstOrDefault(string channelId) => Get().FirstOrDefault(x => x.Id == channelId).ToCommonChannel();
    }
}
