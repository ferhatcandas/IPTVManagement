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
        public bool IsHalfIntegrated => IntegrationType.Equals(Model.IntegrationType.Half.ToString(), StringComparison.InvariantCultureIgnoreCase);
    }
    public enum IntegrationType
    {
        Fixed,
        Half,
        Full
    }
}
