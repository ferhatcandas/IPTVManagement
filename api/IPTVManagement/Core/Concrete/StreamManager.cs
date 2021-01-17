using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Core.Concrete
{
    public class StreamManager
    {
        

        internal byte[] Export(List<CommonChannelModel> newChannels)
        {
            return UploadFile(GenerateM3UChannelFile(newChannels.Where(x=>x.HasStream).ToList()));
        }

        private byte[] UploadFile(string text)
        {
            SaveFile(text);
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
        private void SaveFile(string context)
        {
            File.WriteAllText("channels.m3u", context);
        }
    }
}
