using DataLayer.Repository;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    public class TVChannelRepository : FileRepository<TVChannel>
    {
        public TVChannelRepository() : base("channels")
        {
        }
    }
}
