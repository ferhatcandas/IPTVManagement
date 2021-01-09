using Model;
using System.Collections.Generic;

namespace Core.Abstract
{
    public interface IFixedChannelService
    {
        List<CommonChannelModel> Get();
        void Add(Channel channel);
        void UpdateChannel(string channelId, Channel channel);
        void Delete(string channelId);
        void UpdateStatus(string channelId);
        CommonChannelModel Get(string channelId);
    }
}
