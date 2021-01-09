using Model;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Core.Integrations
{
    public class HTAManager
    {
        public List<CommonChannelModel> Get(HTASettingModel settings)
        {
            List<CommonChannelModel> channels = new List<CommonChannelModel>();
            using (HttpClient client = new HttpClient())
            {
                string token = client.GetAsync(settings.AuthToken).Result.Content.ReadAsStringAsync().Result;

                string tvList = client.GetAsync(settings.Link).Result.Content.ReadAsStringAsync().Result;

                JToken jobject = JObject.Parse(tvList).SelectToken("tiles");

                var list = jobject.ToObject<List<HTAModel>>();

                foreach (var item in list.Where(x => x.channel != null))
                {
                    channels.Add(new CommonChannelModel
                    {
                        Category = "HTA TV",
                        Country = "DZ",
                        Integration = IntegrationType.Full.ToString(),
                        IsActive = true,
                        IsEditable = false,
                        HasStream = true,
                        Language = "Arabic",
                        Logo = item.channel.Picture,
                        Name = item.channel.Name,
                        Stream = item.channel.Url + token,
                        Tags = null
                    });
                }
            }
            return channels;
        }

    }
}
