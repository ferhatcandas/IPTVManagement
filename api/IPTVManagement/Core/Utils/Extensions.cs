using Core.Concrete;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utils
{
    public static class Extensions
    {
        public static List<CommonChannelModel> GetFullIntegratedChannels(this GenericChannelIntegration integration)
        {
            if (!integration.IsHalfIntegrated)
            {
                return Get(integration);
            }
            return null;

        }
        public static List<CommonChannelModel> GetHalfIntegratedChannels(this GenericChannelIntegration integration)
        {
            if (integration.IsHalfIntegrated)
            {
                return Get(integration);
            }
            return null;
        }
        private static List<CommonChannelModel> Get(GenericChannelIntegration integration)
        {
            return IntegrationFactory.Execute(integration);
        }
        private static T GetSettings<T>(this GenericChannelIntegration integration)
        {
            return (T)integration.Settings;
        }
    }
}
