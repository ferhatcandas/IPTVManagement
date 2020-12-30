using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class TVChannel
    {
        public TVChannel()
        {
            Id = Guid.NewGuid().ToString();
        }
        public TVChannel(string name, string logo = "https://www.kindpng.com/picc/m/463-4636269_television-png-transparent-images-transparent-background-tv-logo.png", string language = "Turkish", string category = "", string country = "TR", string stream = "", List<string> tags = null, bool isActive = false, string integration = "")
        {
            Id = Guid.NewGuid().ToString();
            Integration = integration;
            Name = name;
            Logo = logo;
            Language = language;
            Country = country;
            Category = category;
            Stream = stream;
            Tags = tags;
            IsActive = isActive;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Language { get; set; }
        public string Category { get; set; }
        public string Country { get; set; }
        public string Integration { get; set; }
        public string Stream { get; set; }
        public List<string> Tags { get; set; }
        public bool IsActive { get; set; }
        public bool Found => !string.IsNullOrEmpty(Stream);
        public bool Editable => string.IsNullOrEmpty(Integration);

        public M3U8Channel ToM3U()
        {
            return new M3U8Channel(this);
        }
    }
}
