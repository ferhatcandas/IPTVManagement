using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstract
{
    public interface IChannelService
    {
        Task<List<CommonChannelModel>> GetChannels();
        Task<Stream> GetStream();
        Task<CommonChannelModel> GetChannel(string channelId);
        Task UpdateChannel(string channelId, Channel request);
        Task UpdateStatus(string channelId);
        Task DeleteChannel(string channelId);
    }
}
