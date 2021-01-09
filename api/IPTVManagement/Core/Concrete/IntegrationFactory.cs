using Core.Integrations;
using DataLayer.Cache;
using Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Concrete
{
    public class IntegrationFactory
    {
        private readonly HTAManager htaManager;
        private readonly ElahmadManager elahmadManager;
        private readonly HalfIntegrateManager halfIntegrateManager;
        private readonly ChannelController channelController;
        private readonly CacheManager cacheManager;

        public IntegrationFactory(HTAManager hTAManager, ElahmadManager elahmadManager, HalfIntegrateManager halfIntegrateManager, ChannelController channelController, CacheManager cacheManager)
        {
            this.htaManager = hTAManager;
            this.elahmadManager = elahmadManager;
            this.halfIntegrateManager = halfIntegrateManager;
            this.channelController = channelController;
            this.cacheManager = cacheManager;
        }


        internal List<CommonChannelModel> Execute(string name, object setting)
        {
            List<CommonChannelModel> channels = GetFromCache(name);
            if (channels?.Count > 0)
            {
                return channels;
            }

            switch (name)
            {
                case "hta":
                    channels = htaManager.Get(JObject.FromObject(setting).ToObject<HTASettingModel>());
                    Task.Run(() =>
                    {
                        channelController.ControlAndGet(channels);
                        cacheManager.Delete(name);
                        cacheManager.Add(name, channels);
                    });
                    break;
                case "elahmad":
                    channels = elahmadManager.Get(JObject.FromObject(setting).ToObject<ElahmadSettingModel>());
                    Task.Run(() =>
                    {
                        channelController.ControlAndGet(channels);
                        cacheManager.Delete(name);
                        cacheManager.Add(name, channels);
                    });
                    break;
                case "freeiptvlists":
                case "dailyiptvlist":
                    channels = halfIntegrateManager.Get(JObject.FromObject(setting).ToObject<HalfIntegrateSettingModel>());
                    break;
                default:
                    break;
            }

            cacheManager.Add(name, channels, 60);
           
            return channels;
        }
        private List<CommonChannelModel> GetFromCache(string name)
        {
            return cacheManager.Get<List<CommonChannelModel>>(name);
        }
    }
}
