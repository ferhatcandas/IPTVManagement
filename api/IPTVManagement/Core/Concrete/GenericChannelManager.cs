using Core.Abstract;
using Core.Utils;
using DataLayer;
using DataLayer.Cache;
using Model;
using System;
using System.Collections.Generic;

namespace Core.Concrete
{
    public class GenericChannelManager : IGenericChannelService
    {
        private readonly GenericChannelRepository channelRepository;
        private readonly CacheManager cacheManager;

        public GenericChannelManager(GenericChannelRepository channelRepository, CacheManager cacheManager)
        {
            this.channelRepository = channelRepository;
            this.cacheManager = cacheManager;
        }
        public List<CommonChannelModel> Get()
        {
            List<CommonChannelModel> channels = new List<CommonChannelModel>();
            return channels;
        }

        public List<CommonChannelModel> GetFullIntegratedChannels()
        {
            List<CommonChannelModel> channels = cacheManager.Get<List<CommonChannelModel>>("full");
            if (channels?.Count > 0)
            {
                return channels;
            }
            else
            {
                channels = new List<CommonChannelModel>();
            }

            List<GenericChannelIntegration> genericSettings = channelRepository.Get();

            foreach (GenericChannelIntegration integration in genericSettings)
            {
                channels.AddRange(integration.GetFullIntegratedChannels());
            }
            cacheManager.Add("half", channels);
            return channels;
        }

        public List<CommonChannelModel> GetHalfIntegratedChannels()
        {
            List<CommonChannelModel> channels = cacheManager.Get<List<CommonChannelModel>>("half");
            if (channels?.Count > 0)
            {
                return channels;
            }
            else
            {
                channels = new List<CommonChannelModel>();
            }

            List<GenericChannelIntegration> genericSettings = channelRepository.Get();

            foreach (GenericChannelIntegration integration in genericSettings)
            {
                channels.AddRange(integration.GetHalfIntegratedChannels());
            }
            cacheManager.Add("half", channels);
            return channels;
        }
    }
}
