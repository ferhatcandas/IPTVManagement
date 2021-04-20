using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Settings
    {
        public MongoDBSettings MongoDBSettings { get; set; }
    }
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }
}
