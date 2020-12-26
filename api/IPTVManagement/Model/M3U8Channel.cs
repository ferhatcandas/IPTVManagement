using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class M3U8Channel
    {
        public string ChannelName { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string StreamLink { get; set; }
        public string Logo { get; set; }
        public string Url { get; set; }
        public string CategoryName { get; set; }
        public bool IsAddList { get; set; }

        public TVChannel ToTVChannel(bool isActive = false)
        {
            return new TVChannel(ChannelName, null, isActive, StreamLink, CategoryName, Language, Logo, Country);
        }
    }

}
