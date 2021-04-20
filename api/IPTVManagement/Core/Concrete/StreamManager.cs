using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Core.Concrete
{
    public class StreamManager
    {
        internal async Task<byte[]> Export(List<CommonChannelModel> newChannels)
        {
            return await UploadFile(GenerateM3UChannelFile(newChannels.Where(x => x.HasStream).ToList()));
        }

        private async Task<byte[]> UploadFile(string text)
        {
            await SaveFile(text);
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            return bytes;
        }

        private string GenerateM3UChannelFile(List<CommonChannelModel> channels)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("#EXTM3U");
            stringBuilder.Append("\n");
            foreach (CommonChannelModel item in channels)
            {
                stringBuilder.Append(GenerateChannel(item));
                stringBuilder.Append("\n");
            }
            return stringBuilder.ToString();
        }
        private string GenerateChannel(CommonChannelModel channel)
        {
            return $"#EXTINF:-1 tvg-id=\"{channel.Name}\" tvg-name=\"{channel.Name}\" tvg-language=\"{channel.Language}\" tvg-logo=\"{channel.Logo}\" tvg-country=\"{channel.Country}\" tvg-url=\"\" group-title=\"{channel.Category}\",{channel.Name}\n{channel.Stream}";
        }
        private async Task SaveFile(string context)
        {
            await File.WriteAllTextAsync("channels.m3u", context);
        }
    }
}
