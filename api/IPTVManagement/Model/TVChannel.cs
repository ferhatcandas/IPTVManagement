using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class TVChannel
    {
        public TVChannel(string name, string[] tags, string link = null, string category = "", string language = "", string logo = "", string country = "")
        {
            Id = Guid.NewGuid().ToString();
            ShowChannelName = name;
            ChannelTags = tags;
            IsActive = true;
            Logo = logo;
            Language = language;
            Category = category;
            Country = country;
            StreamLink = link;
            Found = !string.IsNullOrEmpty(link);

        }
        public TVChannel()
        {

        }
        public string Id { get; set; }
        public string[] ChannelTags { get; set; }
        public string ShowChannelName { get; set; }
        public bool IsActive { get; set; }
        public string Logo { get; set; }
        public string Language { get; set; }
        public string Category { get; set; }
        public string Country { get; set; }
        public string StreamLink { get; set; }
        public bool Found { get; set; }
        public M3U8Channel ToM3U()
        {
            return new M3U8Channel
            {
                CategoryName = this.Category,
                ChannelName = this.ShowChannelName,
                Country = this.Country,
                Language = this.Language,
                StreamLink=this.StreamLink,
                Logo = this.Logo,
                Url = null
            };
        }
    }
}
