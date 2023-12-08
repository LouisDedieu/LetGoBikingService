using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace ProxyCacheSOAP.Service
{


    public class GenericProxyCache<T>
    {
        private ObjectCache cache = MemoryCache.Default;
        public DateTimeOffset dt_default = ObjectCache.InfiniteAbsoluteExpiration;

        public T Get(string cacheItemName, Func<T> createItem, double dt_minutes)
        {
            if (cache.Contains(cacheItemName))
            {
                return (T)cache[cacheItemName];
            }
            else
            {
                DateTimeOffset expiration = dt_minutes == double.MaxValue ? dt_default : DateTimeOffset.Now.AddMinutes(dt_minutes);
                T newItem = createItem();
                cache.Set(cacheItemName, newItem, expiration);
                return newItem;
            }
        }
    }

}
