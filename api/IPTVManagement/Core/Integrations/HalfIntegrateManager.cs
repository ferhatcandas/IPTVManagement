using Core.Concrete;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Integrations
{
    public class HalfIntegrateManager
    {
        private readonly M3UManager m3UManager;

        public HalfIntegrateManager(M3UManager m3UManager)
        {
            this.m3UManager = m3UManager;
        }
        public List<CommonChannelModel> Get(HalfIntegrateSettingModel settings)
        {
            List<CommonChannelModel> channels = new List<CommonChannelModel>();
            List<string> links = GetLinks(settings);

            foreach (string item in links)
            {
                List<M3U8Channel> list = m3UManager.DownloadAndGet(item, IntegrationType.Half.ToString());

                channels.AddRange(list.Select(x => x.ToCommanChannel()));
            }
            return channels;
        }
        private List<string> GetLinks(HalfIntegrateSettingModel settings)
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
