using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstract
{
    public interface IScheduleService
    {
        Task Syncronize();
        Task TransferChannels();
    }
}
