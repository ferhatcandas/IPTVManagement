using Core.Integrations.Abstract;
using DataLayer.Repository.Mongo.Abstract;
using Model;
using Model.Integration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Core.Integrations.Concrete
{
    public class HTAStrategy : IIntegration
    {
        private readonly HttpClient httpClient;
        private readonly IIntegrationRepository<HTASettings> repository;

        public HTAStrategy(
            HttpClient httpClient,
            IIntegrationRepository<HTASettings> repository
            )
        {
            this.httpClient = httpClient;
            this.repository = repository;
        }
        public async Task<List<CommonChannelModel>> GetAsync()
        {
            var setting = await repository.GetFirstAsync();
            var settings = setting.Settings;
            List<CommonChannelModel> channels = new List<CommonChannelModel>();

            string token = await (await httpClient.GetAsync(settings.AuthToken)).Content.ReadAsStringAsync();

            string tvList = await (await httpClient.GetAsync(settings.Link)).Content.ReadAsStringAsync();

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
            return channels;
        }
    }
}
