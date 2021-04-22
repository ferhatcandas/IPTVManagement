using Core.Concrete;
using Core.Integrations.Abstract;
using DataLayer.Repository.Mongo.Abstract;
using Model;
using Model.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Integrations.Concrete
{
    public class HalfIntegratedStrategy : IIntegration
    {
        private readonly IIntegrationRepository<HalfIntegratedSettings> repository;
        private readonly M3UManager manager;

        public HalfIntegratedStrategy(
            IIntegrationRepository<HalfIntegratedSettings> repository,
            M3UManager manager)
        {
            this.repository = repository;
            this.manager = manager;
        }


        public async Task<List<CommonChannelModel>> GetAsync()
        {
            var setting = await repository.GetAsync(x=>x.Type == nameof(HalfIntegratedStrategy));
            List<CommonChannelModel> channels = new List<CommonChannelModel>();

            foreach (var item in setting)
            {
                List<string> links = GetLinks(item.Settings);

                foreach (string link in links)
                {
                    List<M3U8Channel> list = manager.DownloadAndGet(link, nameof(HalfIntegratedStrategy));

                    channels.AddRange(list.Select(x => x.ToCommanChannel()));
                }
            }
            
            return channels;
        }
        private List<string> GetLinks(HalfIntegratedSettings settings)
        {

            List<string> links = new List<string>();

            if (settings.HasScheme())
            {
                switch (settings.SchemeType)
                {
                    case "date":
                        if (settings.LastPartIndex != null)
                        {
                            for (int i = 1; i < settings.LastPartIndex; i++)
                            {
                                if (settings.Periods.Contains("yesterday"))
                                {
                                    links.Add(settings.Scheme.Replace("{date}", DateTime.Now.AddDays(-1).ToString(settings.DateFormat)).Replace("{part}", i.ToString()));
                                }
                                if (settings.Periods.Contains("today"))
                                {
                                    links.Add(settings.Scheme.Replace("{date}", DateTime.Now.ToString(settings.DateFormat)).Replace("{part}", i.ToString()));
                                }
                            }
                        }
                        else
                        {
                            if (settings.Periods.Contains("yesterday"))
                            {
                                links.Add(settings.Scheme.Replace("{date}", DateTime.Now.AddDays(-1).ToString(settings.DateFormat)));
                            }
                            if (settings.Periods.Contains("today"))
                            {
                                links.Add(settings.Scheme.Replace("{date}", DateTime.Now.ToString(settings.DateFormat)));
                            }
                        }
                        return links;
                    default:
                        break;
                }
            }
            return links;
        }
    }
}
