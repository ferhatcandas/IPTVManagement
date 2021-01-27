using Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Core.Concrete
{
    public class ChannelController
    {
        private readonly HttpClient httpClient;
        public ChannelController()
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            this.httpClient = new HttpClient(httpClientHandler);
        }

        private HttpResponseMessage GetResponse(string url)
        {
            return httpClient.GetAsync(url).Result;
        }
        private void GetLastUrl(CommonChannelModel channelModel)
        {
            var response = GetResponse(channelModel.Stream);
            if ((response.StatusCode == HttpStatusCode.Moved || response.StatusCode == HttpStatusCode.MovedPermanently || response.StatusCode == HttpStatusCode.Found) && !string.IsNullOrEmpty(response.Headers?.Location?.AbsoluteUri))
            {
                channelModel.Stream = response.Headers.Location.AbsoluteUri;
                GetLastUrl(channelModel);
            }
            else
            {
                response = GetResponse(channelModel.Stream);
                channelModel.StatusCode = response.StatusCode.ToString();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    channelModel.Stream = "";
                }
            }

        }
        internal void ControlAndGet(List<CommonChannelModel> filteredChannels)
        {

            foreach (CommonChannelModel channel in filteredChannels)
            {
                channel.HasStream = false;
                int count = 0;
                again:
                try
                {
                    GetLastUrl(channel);
                    count++;
                }
                catch (Exception)
                {
                    count++;
                }
                bool success = false;
                if (!string.IsNullOrEmpty(channel.Stream))
                {

                    success = true;
                    channel.HasStream = true;
                }
                if (!success && count < 2)
                {
                    goto again;
                }
            }
        }
    }
}
