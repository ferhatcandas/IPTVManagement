using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utils
{
    public static class Extensions
    {
        public static List<CommonChannelModel> GetCommonChannels(this GenericChannelIntegration integration)
        {
            switch (integration.IntegrationType.ToLower())
            {
                case "hta":
                    break;
                case "elahmad":
                    break;
                case "dailyiptvlist":
                    break;
                case "freeiptvlists":
                    break;
                default:
                    break;
            }
            return null;
        }
    }
}
