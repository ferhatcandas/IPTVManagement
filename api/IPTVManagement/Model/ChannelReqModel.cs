using System;
namespace Model
{
    public class TVChannelModel
    {
        public string Name { get; set; }
        public string Tags { get; set; }
        public bool IsActive { get; set; }
        public string Logo { get; set; }
        public string Language { get; set; }
        public string Category { get; set; }
        public string Country { get; set; }
        public string StreamLink { get; set; }
        public TVChannel ToTvChannel(string channelId)
        {
            var channel = new TVChannel(Name, Tags?.Split('|'), IsActive, StreamLink, Category, Language, Logo, Country);
            channel.Id = channelId;
            return channel;
        }
    }
}