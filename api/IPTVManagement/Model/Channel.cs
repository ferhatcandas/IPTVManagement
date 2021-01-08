using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Channel : BaseClass
    {
        public string Name { get; set; }
        public string Stream { get; set; }
        public string Category { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public bool IsActive { get; set; }
    }
}
