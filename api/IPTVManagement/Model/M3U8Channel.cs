using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class M3U8Channel
    {
        public M3U8Channel(string name, string language, string country, string category, string logo = "https://www.kindpng.com/picc/m/463-4636269_television-png-transparent-images-transparent-background-tv-logo.png", string url = null, string integration = null)
        {
            ChannelName = name;
            Language = language;
            Country = country;
            CategoryName = category;
            Logo = logo;
            Url = url;
            Integration = integration;
        }
        public M3U8Channel(HTAModel htaModel, string token)
        {
            CategoryName = "HTA TV";
            ChannelName = htaModel.channel.Name;
            Country = "DZ";
            Language = "Arabic";
            Logo = htaModel.channel.Picture;
            StreamLink = htaModel.channel.Url + token;
            Integration = "HTA";
        }
        public M3U8Channel(TVChannel tVChannel)
        {
            ChannelName = tVChannel.Name;
            Language = tVChannel.Language;
            Country = tVChannel.Country;
            StreamLink = tVChannel.Stream;
            Logo = tVChannel.Logo;
            Url = null;
            CategoryName = tVChannel.Category;
            Integration = null;
        }
        public string ChannelName { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string StreamLink { get; set; }
        public string Logo { get; set; }
        public string Url { get; set; }
        public string CategoryName { get; set; }
        public string Integration { get; set; }
        public bool HasIntegration => !string.IsNullOrEmpty(Integration);

        public TVChannel ToTVChannel(bool isActive = false, string integration = "") => new TVChannel(ChannelName, Logo, Language, CategoryName, Country, StreamLink, null, isActive, integration);
    }

}
