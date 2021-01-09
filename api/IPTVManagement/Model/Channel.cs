using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Channel : BaseChannel
    {
        public string Id { get; set; }
        public string Tags { get; set; }
        public bool IsActive { get; set; }

        public CommonChannelModel ToCommonChannel()
        {
            return new CommonChannelModel
            {
                Category = Category,
                Country = Country,
                Id = Id,
                Integration = IntegrationType.Fixed.ToString(),
                IsActive = IsActive,
                HasStream = true,
                IsEditable = true,
                Language = Language,
                Logo = Logo,
                Name = Name,
                Stream = Stream,
                Tags = Tags
            };
        }
    }
}
