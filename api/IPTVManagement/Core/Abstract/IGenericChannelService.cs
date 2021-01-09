using Model;
using System.Collections.Generic;

namespace Core.Abstract
{
    public interface IGenericChannelService
    {
        List<CommonChannelModel> GetHalfIntegratedChannels(IEnumerable<CommonChannelModel> includeHalfIntegrated);
        List<CommonChannelModel> GetFullIntegratedChannels();

    }
}
