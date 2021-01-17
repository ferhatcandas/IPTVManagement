using Model;
using System.Collections.Generic;

namespace Core.Abstract
{
    public interface IGenericChannelService
    {
        List<CommonChannelModel> GetHalfIntegratedChannels(IEnumerable<CommonChannelModel> includeHalfIntegrated, bool reCache = false);
        List<CommonChannelModel> GetFullIntegratedChannels(bool reCache=false);
    }
}
