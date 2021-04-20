using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Integration
{
    public class HTASettings : IntegrationSettings
    {
        public string Link { get; set; }
        public string AuthToken { get; set; }
    }
    public class HTAModel
    {
        public HTAChannel channel { get; set; }
    }
    public class HTAChannel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Picture { get; set; }
    }
}
