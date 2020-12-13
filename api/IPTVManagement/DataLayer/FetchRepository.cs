using DataLayer.Repository;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    public class FetchRepository : FileRepository<FetchLink>
    {
        public FetchRepository():base("fetchlinks")
        {
        }
    }
}
