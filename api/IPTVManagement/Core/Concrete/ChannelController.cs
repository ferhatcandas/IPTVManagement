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
        internal void ControlAndGet(List<CommonChannelModel> filteredChannels)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            HttpClient httpClient2 = new HttpClient(httpClientHandler);
            foreach (CommonChannelModel channel in filteredChannels)
            {
                channel.HasStream = false;
                int count = 0;
                HttpResponseMessage response = null;
                again:
                try
                {

                    response = httpClient2.GetAsync(channel.Stream).Result;
                    count++;
                }
                catch (Exception)
                {
                    count++;
                }
                bool success = false;
                if (response != null && (response.StatusCode == HttpStatusCode.Found || response.StatusCode == HttpStatusCode.MovedPermanently || response.StatusCode == HttpStatusCode.Moved || response.StatusCode == HttpStatusCode.OK))
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
