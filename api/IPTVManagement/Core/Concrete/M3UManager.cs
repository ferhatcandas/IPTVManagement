using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Concrete
{
    public class M3UManager
    {
        private readonly HttpClient httpClient;

        public M3UManager(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public List<M3U8Channel> DownloadAndGet(string url, string integration)
        {
            DownloadPlayList(url, out string output);
            if (output != null)
            {
                return GetFromText(output, integration);

            }
            return new List<M3U8Channel>();

        }
        private bool DownloadPlayList(string url, out string outPutText)
        {
            HttpResponseMessage response = null;
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 0, 15);
                response = client.GetAsync(url).Result;
            }


            if (response.IsSuccessStatusCode)
            {
                using (FileStream fs = new FileStream($"playlist.txt", FileMode.CreateNew))
                {
                    response.Content.CopyToAsync(fs).GetAwaiter().GetResult();
                }

                outPutText = File.ReadAllText("playlist.txt");

                File.Delete("playlist.txt");
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
                MatchCollection matchCollection = Regex.Matches(text.Replace("ᶫᵒ", ""), "((#EXTINF:.*)|(((rtmp)|(http)).*)).*", RegexOptions.Multiline | RegexOptions.Compiled);

                if (matchCollection.Count % 2 == 0)
                {
                    for (int i = 0; i < matchCollection.Count; i += 2)
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
            static string GetValue(MatchCollection matches, string paramet)
            {
                string match = null;

                foreach (object item in matches)
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
            string channelName = splittedText[^1].Trim();
            MatchCollection matchCollection = Regex.Matches(firstLine, "(([\\w|-]*|)=\"[\\S]*|(\"))");

            M3U8Channel channel = new M3U8Channel(channelName, link.Trim(), GetValue(matchCollection, "tvg-language").Trim(), GetValue(matchCollection, "tvg-country").Trim(), integration, GetValue(matchCollection, "tvg-logo").Trim());
            return channel;
        }


    }
}
