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

        public M3UManager(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.httpClient.Timeout = new TimeSpan(0, 0, 15);
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

                    success = true;
                    activeChannels.Add(channel);
                }
                if (!success && count < 2)
                {
                    goto again;
                }
            }
            return activeChannels;
        }

        internal byte[] Export(List<M3U8Channel> newChannels)
        {
            return UploadFile(GenerateM3UChannelFile(newChannels));
        }

        private byte[] UploadFile(string text)
        {
            SaveFile(text);
            var bytes = Encoding.UTF8.GetBytes(text);
            return bytes;
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

        private void SaveFile(string context)
        {
            File.WriteAllText("channels.m3u", context);
        }

    }
}
