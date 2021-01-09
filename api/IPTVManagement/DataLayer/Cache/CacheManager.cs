using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Text;

namespace DataLayer.Cache
{
    public class CacheManager
    {
        private static readonly ObjectCache cache = MemoryCache.Default;
        public void Add<T>(string key, T model, int minutes = 20)
        {
            cache.Add(key, model, DateTime.Now.AddMinutes(minutes));
        }
        
        public void Delete(string key)
        {
            cache.Remove(key);
        }
        public T Get<T>(string key) => (T)cache.Get(key);
    }
}
