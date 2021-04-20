using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Integrations.Abstract
{
    public interface IIntegration
    {
        Task<List<CommonChannelModel>> GetAsync();
    }
}
