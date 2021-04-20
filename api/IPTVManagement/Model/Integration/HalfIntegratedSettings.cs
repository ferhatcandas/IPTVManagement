using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Integration
{
    public class HalfIntegratedSettings : IntegrationSettings
    {
        public string SchemeType { get; set; }
        public int? LastPartIndex { get; set; }
        public List<string> Periods { get; set; }
        public string Scheme { get; set; }
        public string DateFormat { get; set; }

        public bool HasScheme() => !string.IsNullOrEmpty(Scheme);
    }
}
