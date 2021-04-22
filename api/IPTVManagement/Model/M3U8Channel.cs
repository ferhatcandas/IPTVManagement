using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class M3U8Channel : BaseChannel
    {
        public M3U8Channel(string channelName, string link, string language, string country, string integration, string logo)
        {
            Name = channelName;
            Stream = link;
            Integration = integration;
            Country = country;
            Language = language;
            Logo = logo;
        }

        public string Integration { get; set; }
        public CommonChannelModel ToCommanChannel()
        {
            return new CommonChannelModel
            {
                Category = Category,
                Country = Country,
                Integration = Integration,
                IsActive = true,
                IsEditable = false,
                Language = Language,
                Logo = Logo,
                Name = Name,
                Stream = Stream,
                Tags = null,
                Id = Guid.NewGuid().ToString()
            };
        }
    }
}
