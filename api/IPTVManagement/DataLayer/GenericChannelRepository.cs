using DataLayer.Repository;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    public class GenericChannelRepository : FileRepository<GenericChannelIntegration>
    {
        public GenericChannelRepository() : base("generic")
        {
        }
    }
}
