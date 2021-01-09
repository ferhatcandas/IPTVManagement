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
            fixedChannel.Add(channel);
        }

        public void UpdateChannel(string channelId, Channel channel)
        {
            fixedChannel.UpdateChannel(channelId, channel);
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
            var channels = GetChannels(true);
            var bytes = streamManager.Export(channels);

            return new MemoryStream(bytes);
        }
        public List<CommonChannelModel> GetChannels(bool additional = true)
        {
            List<CommonChannelModel> fixedChannels = fixedChannel.Get();

            var includeHalfIntegrated = fixedChannels.Where(x => x.IsHalfIntegrated());

            if (includeHalfIntegrated.Count() > 0)
            {
                List<CommonChannelModel> halfIntegratedChannels = genericChannel.GetHalfIntegratedChannels(includeHalfIntegrated);
                SprayChannels(fixedChannels, halfIntegratedChannels);
            }

            if (additional)
            {
                List<CommonChannelModel> fullIntegratedChannels = genericChannel.GetFullIntegratedChannels();
                fixedChannels.AddRange(fullIntegratedChannels);

            }
            return fixedChannels;
        }
       
       

        public CommonChannelModel GetChannel(string channelId)
        {
            return fixedChannel.Get(channelId);
        }
    }
}
