using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Integration
{
    public class IntegrationBase<T> : IMongoEntity
        where T : IntegrationSettings
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IntegrationType Type { get; set; }
        public T Settings { get; set; }
    }
}
