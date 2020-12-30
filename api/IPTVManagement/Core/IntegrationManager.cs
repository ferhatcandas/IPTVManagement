using DataLayer;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace Core
{
    public class IntegrationManager
    {
        private readonly HttpClient httpClient;
        private readonly IntegrationRepository integrationRepository;

        public IntegrationManager(HttpClient httpClient, IntegrationRepository integrationRepository)
        {
            this.httpClient = httpClient;
            this.integrationRepository = integrationRepository;
        }
        public List<M3U8Channel> FetchChannels()
        {
            var settings = integrationRepository.Get();
            List<M3U8Channel> channels = new List<M3U8Channel>();

            foreach (var setting in settings)
            {
                var links = setting.GetLinks();
                foreach (var item in links)
                {
                    channels.AddRange(DownloadAndGet(item, setting.Integration));
                }
                channels.AddRange(setting.GetLinksModelled());
            }
            return channels;
        }
        public List<M3U8Channel> DownloadAndGet(string url, string integration)
        {
            string output = "";
            DownloadPlayList(url, out output);
            if (output != null)
            {
                return GetFromText(output,integration);
            }
            return new List<M3U8Channel>();

        }
        private bool DownloadPlayList(string url, out string outPutText)
        {
            var response = httpClient.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                using (var fs = new FileStream($"test.txt", FileMode.CreateNew))
                    response.Content.CopyToAsync(fs).GetAwaiter().GetResult();

                outPutText = File.ReadAllText("test.txt");

                File.Delete("test.txt");
                return true;
            }
            outPutText = null;
            return false;
        }
        private List<M3U8Channel> GetFromText(string text, string integration)
        {
            List<M3U8Channel> list = new List<M3U8Channel>();

            try
            {
                var matchCollection = Regex.Matches(text.Replace("ᶫᵒ", ""), "((#EXTINF:.*)|(((rtmp)|(http)).*)).*", RegexOptions.Multiline | RegexOptions.Compiled);

                if (matchCollection.Count % 2 == 0)
                {
                    for (int i = 0; i < matchCollection.Count; i = i + 2)
                    {
                        M3U8Channel mu8Channel = GetM3U(matchCollection[i].Groups, matchCollection[i + 1].Value, integration);
                        list.Add(mu8Channel);
                    }
                }
            }
            catch (Exception)
            {

            }

            return list;
        }
        private M3U8Channel GetM3U(GroupCollection groups, string link, string integration)
        {
            string GetValue(MatchCollection matches, string paramet)
            {
                string match = null;

                foreach (var item in matches)
                {
                    if (item.ToString().Contains(paramet))
                    {
                        match = item.ToString();
                        break;
                    }
                }
                if (match != null)
                {
                    return match.Replace(paramet, "").Replace("=", "").Replace("\"", "");
                }
                return "";
            }
            string[] splittedText = groups[0].Value.Split(',');
            string firstLine = splittedText[0];
            string channelName = splittedText[splittedText.Length - 1].Trim();
            var matchCollection = Regex.Matches(firstLine, "(([\\w|-]*|)=\"[\\S]*|(\"))");

            M3U8Channel channel = new M3U8Channel(channelName, GetValue(matchCollection, "tvg-language").Trim(), GetValue(matchCollection, "tvg-country").Trim(), integration, GetValue(matchCollection, "tvg-logo").Trim());
            return channel;
        }

        internal IEnumerable<TVChannel> GetAddionalList()
        {
            var settings = integrationRepository.Get().Where(x => x.Addional);
            List<M3U8Channel> channels = new List<M3U8Channel>();

            foreach (var setting in settings)
            {
                channels.AddRange(setting.GetLinksModelled());
            }
            return channels.Select(x => x.ToTVChannel(true, x.Integration));
        }
    }
}
