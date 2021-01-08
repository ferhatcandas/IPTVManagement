using Core.Integrations;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Concrete
{
    public class IntegrationFactory
    {
        private readonly HTAManager htaManager;
        private readonly ElahmadManager elahmadManager;
        private readonly HalfIntegrateManager halfIntegrateManager;

        public IntegrationFactory(HTAManager hTAManager, ElahmadManager elahmadManager, HalfIntegrateManager halfIntegrateManager)
        {
            this.htaManager = hTAManager;
            this.elahmadManager = elahmadManager;
            this.halfIntegrateManager = halfIntegrateManager;
        }


        internal List<CommonChannelModel> Execute<T>(string name, T setting)
            where T : class, new()
        {
            switch (name)
            {
                case "hta":
                    return htaManager.Get((HTASettingModel)Convert.ChangeType(setting, typeof(HTASettingModel)));
                case "elahmad":
                    return elahmadManager.Get((ElahmadSettingModel)Convert.ChangeType(setting, typeof(ElahmadSettingModel)));
                case "freeiptvlists":
                    return halfIntegrateManager.Get((HalfIntegrateSettingModel)Convert.ChangeType(setting, typeof(HalfIntegrateSettingModel)));
                case "dailyiptvlist":
                    return halfIntegrateManager.Get((HalfIntegrateSettingModel)Convert.ChangeType(setting, typeof(HalfIntegrateSettingModel)));
                default:
                    break;
            }
            return null;
        }
    }
}
