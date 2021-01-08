using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class GenericChannelIntegration : BaseClass
    {
        public string IntegrationType { get; set; }
        public string Name { get; set; }
        public object Settings { get; set; }
    }
}
