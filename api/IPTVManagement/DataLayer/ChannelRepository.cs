using DataLayer.Repository;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    public class ChannelRepository : FileRepository<Channel>
    {
        public ChannelRepository() : base("channels")
        {
        }
    }
}
