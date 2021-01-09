using Core.Abstract;
using DataLayer;
using DataLayer.Cache;
using Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Concrete
{
    public class GenericChannelManager : IGenericChannelService
    {
        private readonly GenericChannelRepository genericRepository;
        private readonly IntegrationFactory integrationFactory;
        private readonly CacheManager cacheManager;
        private readonly ChannelController channelController;

        public GenericChannelManager(GenericChannelRepository genericRepository, IntegrationFactory integrationFactory, CacheManager cacheManager, ChannelController channelController)
        {
            this.genericRepository = genericRepository;
            this.integrationFactory = integrationFactory;
            this.cacheManager = cacheManager;
            this.channelController = channelController;
        }
        public List<CommonChannelModel> GetFullIntegratedChannels()
        {
            List<CommonChannelModel> channels = new List<CommonChannelModel>();

            List<GenericChannelIntegration> genericSettings = genericRepository.Get().Where(x => !x.IsHalfIntegrated).ToList();

            foreach (GenericChannelIntegration integration in genericSettings)
            {
                channels.AddRange(integrationFactory.Execute(integration.Name, integration.Settings));
            }
            return channels;
        }

        public List<CommonChannelModel> GetHalfIntegratedChannels(IEnumerable<CommonChannelModel> includeHalfIntegrated)
        {
            List<CommonChannelModel> channels = cacheManager.Get<List<CommonChannelModel>>(IntegrationType.Half.ToString());

            List<GenericChannelIntegration> genericSettings = genericRepository.Get().Where(x => x.IsHalfIntegrated).ToList();
            List<CommonChannelModel> filt = null;
            if (channels?.Count == 0)
            {
                foreach (GenericChannelIntegration integration in genericSettings)
                {
                    var list = integrationFactory.Execute(integration.Name, integration.Settings);
                    filt = Filter(includeHalfIntegrated, list);
                    channels.AddRange(filt);
                }
                cacheManager.Add(IntegrationType.Half.ToString(), channels);
            }
            else
            {
                filt = Filter(includeHalfIntegrated, channels);
            }

            CheckAvailable(channels, filt);

            return channels;
        }
        private void CheckAvailable(List<CommonChannelModel> existChannels, List<CommonChannelModel> checkList)
        {
            Task.Run(() =>
            {
                channelController.ControlAndGet(checkList);

                foreach (var exist in existChannels)
                {
                    var isExist = checkList.FirstOrDefault(x => x.Stream.Trim() == exist.Stream.Trim());
                    if (isExist != null)
                    {
                        exist.HasStream = isExist.HasStream;
                    }
                }
                cacheManager.Delete(IntegrationType.Half.ToString());
                cacheManager.Add(IntegrationType.Half.ToString(), existChannels);
            });
        }
        public List<CommonChannelModel> Filter(IEnumerable<CommonChannelModel> filtredChannels, IEnumerable<CommonChannelModel> existChannels)
        {
            List<CommonChannelModel> channels = new List<CommonChannelModel>();

            foreach (var halfIntegrated in existChannels)
            {
                foreach (var filter in filtredChannels)
                {
                    var tags = filter.Tags.Split(',');
                    foreach (var tag in tags)
                    {
                        if (halfIntegrated.Name.Contains(tag))
                        {
                            filter.Stream = halfIntegrated.Stream;
                            var clonedChannel = filter.Clone();
                            clonedChannel.Stream = halfIntegrated.Stream;
                            clonedChannel.Integration = IntegrationType.Half.ToString();
                            channels.Add(clonedChannel);
                        }
                    }
                }
            }
            return channels;

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
    }
}
