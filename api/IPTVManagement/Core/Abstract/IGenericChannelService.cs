using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace Core.Abstract
{
    public interface IGenericChannelService
    {
        List<CommonChannelModel> GetHalfIntegratedChannels();
        List<CommonChannelModel> GetFullIntegratedChannels();

    }
}
