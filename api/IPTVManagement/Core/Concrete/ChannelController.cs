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
        private void GetLastUrl(CommonChannelModel channelModel, out bool hasStream)
        {
            hasStream = false;
            var response = GetResponse(channelModel.Stream);
            if ((response.StatusCode == HttpStatusCode.Moved || response.StatusCode == HttpStatusCode.MovedPermanently || response.StatusCode == HttpStatusCode.Found) && !string.IsNullOrEmpty(response.Headers?.Location?.AbsoluteUri))
            {
                if (channelModel.Integration != "Fixed")
                    channelModel.Stream = response.Headers.Location.AbsoluteUri;
                GetLastUrl(channelModel, out hasStream);
            }
            else
            {
                channelModel.StatusCode = response.StatusCode.ToString();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    hasStream = false;
                }
                else if (response.StatusCode == HttpStatusCode.OK)
                {
                    hasStream = true;
                }
            }

        }
        internal void ControlAndGet(List<CommonChannelModel> filteredChannels)
        {

            foreach (CommonChannelModel channel in filteredChannels)
            {
                bool hasStream = false;

                channel.HasStream = hasStream;
                int count = 0;
                again:
                try
                {
                    GetLastUrl(channel, out hasStream);
                    count++;
                }
                catch (Exception ex)
                {
                    count++;
                }
                bool success = false;
                if (!string.IsNullOrEmpty(channel.Stream) && hasStream)
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
