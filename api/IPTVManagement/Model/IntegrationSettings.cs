using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class IntegrationSettings
    {
        public string Integration { get; set; }
        public Settings Settings { get; set; }
        public bool Addional { get; set; }
    }
    public class Settings
    {
        public string Link { get; set; }
        public List<string> Periods { get; set; }
        public string Scheme { get; set; }
        public int? LastPartIndex { get; set; }
        public string DateFormat { get; set; }
        public string SchemeType { get; set; }
        public string AuthToken { get; set; }
        public bool HasScheme()
        {
            return !string.IsNullOrEmpty(Scheme);
        }
    }
}
