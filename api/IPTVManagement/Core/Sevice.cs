using Core.Abstract;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Core
{
    public class Sevice
    {
        private readonly IFixedChannelService fixedChannel;
        private readonly IGenericChannelService genericChannel;

        public Sevice(IFixedChannelService fixedChannel, IGenericChannelService genericChannel)
        {
            this.fixedChannel = fixedChannel;
            this.genericChannel = genericChannel;
        }

        void AddChannel(Channel channel)
        {
            fixedChannel.Add(channel);
        }
        void UpdateChannel(Channel channel)
        {

        }
        void DeleteChannel(string channelId)
        {

        }
        void UpdateStatus(string channelId)
        {

        }

        public Stream GetStream()
        {
            return null;
        }
        public List<CommonChannelModel> GetChannels(bool additional = true)
        {
            var fixedChannels = fixedChannel.Get();

            bool includeHalfIntegrated = fixedChannels.Any(x => x.IsHalfIntegrated);

            if (includeHalfIntegrated)
            {
                var halfIntegratedChannels = genericChannel.GetHalfIntegratedChannels();
            }

            if (additional)
            {
                var fullIntegratedChannels = genericChannel.GetFullIntegratedChannels();
            }


            return fixedChannels;
        }




    }
}
