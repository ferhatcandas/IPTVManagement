using DataLayer;
using Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace Core
{
    public class M3UManager
    {
        private readonly HttpClient httpClient;
        private readonly FetchRepository fetchRepository;

        public M3UManager(HttpClient httpClient, FetchRepository fetchRepository)
        {
            this.httpClient = httpClient;
            this.httpClient.Timeout = new TimeSpan(0, 0, 15);
            this.fetchRepository = fetchRepository;
        }
        public List<M3U8Channel> DownloadAndGet(string url)
        {
            string output = "";
            DownloadPlayList(url, out output);
            if (output != null)
            {
                return GetFromText(output);
            }
            return new List<M3U8Channel>();
            //else
            //{
            //    throw new Exception("Source not found " + url);
            //}
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

        internal List<M3U8Channel> ControlAndGetActiveChannels(List<M3U8Channel> filteredChannels)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            var httpClient2 = new HttpClient(httpClientHandler);
            List<M3U8Channel> activeChannels = new List<M3U8Channel>();
            foreach (var channel in filteredChannels)
            {
                int count = 0;
                HttpResponseMessage response = null;
            again:
                try
                {

                    response = httpClient2.GetAsync(channel.StreamLink).Result;
                    count++;
                }
                catch (Exception ex)
                {
                    count++;
                }
                bool success = false;
                if (response != null && (response.StatusCode == HttpStatusCode.Found || response.StatusCode == HttpStatusCode.MovedPermanently || response.StatusCode == HttpStatusCode.Moved || response.StatusCode == HttpStatusCode.OK))
                {
                    //var headers = response.Content.Headers.Where(h => h.Key.Equals("ContentLength"));
                    //int length = 0;
                    //foreach (var item in headers)
                    //{
                    //    if (int.TryParse(item.Value.FirstOrDefault(), out length))
                    //    {
                    success = true;
                    //        break;
                    //    }
                    //}

                    //if (length > 0)
                    //{
                    activeChannels.Add(channel);
                    //}
                }
                if (!success && count < 2)
                {
                    goto again;
                }
            }
            return activeChannels;
        }

        internal void Export(List<M3U8Channel> newChannels)
        {
            UploadFile(GenerateM3UChannelFile(newChannels));
        }

        private void UploadFile(string text)
        {
            SaveFile(text);
            var bytes = Encoding.UTF8.GetBytes(text);

            //using (var client = new HttpClient())
            //{
            //    using (var multipartFormDataContent = new MultipartFormDataContent())
            //    {
            //        var values = new[]
            //        {
            //    new KeyValuePair<string, string>("destination", "/Dahili depolama"),
            //};

            //        foreach (var keyValuePair in values)
            //        {
            //            multipartFormDataContent.Add(new StringContent(keyValuePair.Value),
            //                String.Format("\"{0}\"", keyValuePair.Key));
            //        }

            //        multipartFormDataContent.Add(new ByteArrayContent(bytes),
            //            '"' + "file-0" + '"',
            //            '"' + "channels2.m3u" + '"');

            //        var requestUri = "http://192.168.1.100:1200/fm/uploadUrl";
            //        var result = client.PostAsync(requestUri, multipartFormDataContent).Result;
            //        var responseString = result.Content.ReadAsStringAsync().Result;
            //    }
            //}

            //var content = new MultipartFormDataContent();
            //content.Add(new ByteArrayContent(bytes), "file-0", "channels2.m3u");
            //content.Add(new StringContent("/Dahili depolama"), "destination");

            //var message = httpClient.PostAsync("http://192.168.1.101:1200/fm/uploadUrl", content).Result;
            //message.EnsureSuccessStatusCode();
            //var input = message.Content.ReadAsStringAsync().Result;
        }

        private List<M3U8Channel> GetFromText(string text)
        {
            List<M3U8Channel> list = new List<M3U8Channel>();

            try
            {
                var matchCollection = Regex.Matches(text.Replace("ᶫᵒ", ""), "((#EXTINF:.*)|(((rtmp)|(http)).*)).*", RegexOptions.Multiline | RegexOptions.Compiled);

                if (matchCollection.Count % 2 == 0)
                {
                    for (int i = 0; i < matchCollection.Count; i = i + 2)
                    {
                        M3U8Channel mu8Channel = GetM3U(matchCollection[i].Groups, matchCollection[i + 1].Value);
                        list.Add(mu8Channel);
                    }
                }
            }
            catch (Exception)
            {

            }

            return list;
        }
        private M3U8Channel GetM3U(GroupCollection groups, string link)
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


            M3U8Channel channel = new M3U8Channel
            {
                ChannelName = channelName,
                Country = GetValue(matchCollection, "tvg-country").Trim(),
                Language = GetValue(matchCollection, "tvg-language").Trim(),
                Logo = GetValue(matchCollection, "tvg-logo").Trim(),
                Url = GetValue(matchCollection, "tvg-url").Trim(),
                StreamLink = link.Trim()
            };
            return channel;
        }

        private string GenerateM3UChannelFile(List<M3U8Channel> channels)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("#EXTM3U");
            stringBuilder.Append("\n");
            foreach (var item in channels)
            {
                stringBuilder.Append(GenerateChannel(item));
                stringBuilder.Append("\n");
            }
            return stringBuilder.ToString();
        }
        private string GenerateChannel(M3U8Channel channel)
        {
            return $"#EXTINF:-1 tvg-id=\"{channel.ChannelName}\" tvg-name=\"{channel.ChannelName}\" tvg-language=\"{channel.Language}\" tvg-logo=\"{channel.Logo}\" tvg-country=\"{channel.Country}\" tvg-url=\"\" group-title=\"{channel.CategoryName}\",{channel.ChannelName}\n{channel.StreamLink}";
        }
        public List<M3U8Channel> FetchChannels(bool update = false)
        {
            var urls = fetchRepository.Get();
            List<M3U8Channel> channels = new List<M3U8Channel>();
            foreach (var fetchLink in urls)
            {
                var links = fetchLink.GetLinks();
                foreach (var item in links)
                {
                    channels.AddRange(DownloadAndGet(item));
                }
                fetchLink.LastFetch = DateTime.Now;
            }

            if (update)
            {
                fetchRepository.Save(urls);
            }
            return channels;


        }

        private void SaveFile(string context)
        {
            File.WriteAllText("channels.m3u", context);
        }

    }
}
