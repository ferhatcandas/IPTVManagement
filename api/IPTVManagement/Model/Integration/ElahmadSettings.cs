using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Integration
{
    public class ElahmadSettings : IntegrationSettings
    {
        public List<ElahmadChannel> ChannelLinks { get; set; }
    }
    public class ElahmadChannel
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public string Logo { get; set; }
    }
}
