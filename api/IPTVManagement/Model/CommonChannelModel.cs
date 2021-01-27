using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Model
{
    public class CommonChannelModel : Channel
    {
        public string Integration { get; set; }
        public bool IsEditable { get; set; }
        public bool HasStream { get; set; }
        public string StatusCode { get; set; }
        public bool IsHalfIntegrated() => !string.IsNullOrEmpty(Tags) && string.IsNullOrEmpty(Stream) && IsActive;

        public CommonChannelModel Clone()
        {
            var clonedObject = (CommonChannelModel)this.MemberwiseClone();
            clonedObject.Id = null;
            return clonedObject;
        }
    }
}
