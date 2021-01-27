using Core.Abstract;
using Core.Concrete;
using DataLayer.Cache;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Core
{
    public class Service
    {
        private readonly IFixedChannelService fixedChannel;
        private readonly IGenericChannelService genericChannel;
        private readonly StreamManager streamManager;

        public Service(IFixedChannelService fixedChannel, IGenericChannelService genericChannel, StreamManager streamManager)
        {
            this.fixedChannel = fixedChannel;
            this.genericChannel = genericChannel;
            this.streamManager = streamManager;
        }

        public void AddChannel(Channel channel)
        {
            channel.Id = Guid.NewGuid().ToString();
            fixedChannel.Add(channel);
            if (channel.Tags != null)
            {
                ReCache();
            }
        }

        public void UpdateChannel(string channelId, Channel channel)
        {
            fixedChannel.UpdateChannel(channelId, channel);
            if (channel.Tags != null)
            {
                ReCache();
            }
        }

        public void DeleteChannel(string channelId)
        {
            fixedChannel.Delete(channelId);
        }

        public void UpdateStatus(string channelId)
        {
            fixedChannel.UpdateStatus(channelId);
        }

        public Stream GetStream()
        {
            var channels = GetChannels(true).Where(x=>!string.IsNullOrEmpty(x.Stream)).ToList();
            var bytes = streamManager.Export(channels);

            return new MemoryStream(bytes);
        }
        public List<CommonChannelModel> GetChannels(bool additional = true, bool reCache = false)
        {
            List<CommonChannelModel> fixedChannels = fixedChannel.Get().OrderByDescending(x => x.Id).ToList();

            var includeHalfIntegrated = fixedChannels.Where(x => x.IsHalfIntegrated()).ToList();

            if (includeHalfIntegrated.Count() > 0)
            {
                List<CommonChannelModel> halfIntegratedChannels = genericChannel.GetHalfIntegratedChannels(includeHalfIntegrated, reCache);
                fixedChannels.AddRange(halfIntegratedChannels);
            }

            if (additional)
            {
                List<CommonChannelModel> fullIntegratedChannels = genericChannel.GetFullIntegratedChannels(reCache);
                fixedChannels.AddRange(fullIntegratedChannels);

            }
            return fixedChannels;
        }

        public void ReCache()
        {
            GetChannels(true, true);
        }

        public CommonChannelModel GetChannel(string channelId)
        {
            return fixedChannel.Get(channelId);
        }
    }
}
