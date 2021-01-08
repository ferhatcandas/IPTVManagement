using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Abstract
{
    public interface IFixedChannelService : IChannelIntegration
    {
        void Add(Channel channel);
    }
}
