using DataLayer.Repository;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    public class IntegrationRepository : FileRepository<IntegrationSettings>
    {
        public IntegrationRepository():base("integration")
        {
        }
    }
}
