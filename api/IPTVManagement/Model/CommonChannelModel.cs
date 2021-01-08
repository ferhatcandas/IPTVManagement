using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class CommonChannelModel : Channel
    {
        public string Integration { get; set; }
        public bool IsEditable { get; set; }

        public bool IsHalfIntegrated => !string.IsNullOrEmpty(Tags) && !string.IsNullOrEmpty(Stream);
    }
}
