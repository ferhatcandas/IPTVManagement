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
        public List<CommonChannelModel> GetChannels(bool additional = true, bool reCache = false)
        {
            List<CommonChannelModel> fixedChannels = fixedChannel.Get().OrderByDescending(x => x.Id).ToList();

            var includeHalfIntegrated = fixedChannels.Where(x => x.IsHalfIntegrated());

            if (includeHalfIntegrated.Count() > 0)
            {
                List<CommonChannelModel> halfIntegratedChannels = genericChannel.GetHalfIntegratedChannels(includeHalfIntegrated, reCache);
                SprayChannels(fixedChannels, halfIntegratedChannels);
            }

            if (additional)
            {
                List<CommonChannelModel> fullIntegratedChannels = genericChannel.GetFullIntegratedChannels(reCache);
                fixedChannels.AddRange(fullIntegratedChannels);

            }
            return fixedChannels;
        }
        private void SprayChannels(List<CommonChannelModel> fixedChannels, List<CommonChannelModel> halfIntegratedChannels)
        {
            List<CommonChannelModel> channels = new List<CommonChannelModel>();

            var filtredChannels = fixedChannels.Where(x => x.IsHalfIntegrated());

            foreach (var halfIntegrated in halfIntegratedChannels)
            {
                foreach (var filter in filtredChannels)
                {
                    var tags = filter.Tags.Split(',');
                    foreach (var tag in tags)
                    {
                        if (halfIntegrated.Name.Contains(tag))
                        {
                            if (string.IsNullOrEmpty(filter.Stream))
                            {
                                filter.Stream = halfIntegrated.Stream;
                            }
                            else
                            {
                                var clonedChannel = filter.Clone();
                                clonedChannel.Stream = halfIntegrated.Stream;
                                clonedChannel.Integration = IntegrationType.Half.ToString();
                                channels.Add(clonedChannel);
                            }

                        }
                    }
                }
            }
            fixedChannels.AddRange(channels);
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
