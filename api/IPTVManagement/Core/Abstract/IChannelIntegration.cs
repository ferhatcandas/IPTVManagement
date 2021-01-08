using System;
using System.Collections.Generic;
using Model;

namespace Core.Abstract
{
    public interface IChannelIntegration
    {
        List<CommonChannelModel> Get();
    }
}
