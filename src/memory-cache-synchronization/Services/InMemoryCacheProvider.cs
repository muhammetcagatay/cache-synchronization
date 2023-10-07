using memory_cache_synchronization.Helpers;
using Microsoft.Extensions.Caching.Memory;

namespace memory_cache_synchronization.Services
{
    public interface ICacheProvider
    {
        bool IsExist(string key);
        bool SetOrUpdate(string key, string value);
        T Get<T>(string key);
        bool Remove(string key);
    }

    public class InMemoryCacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryCacheProvider(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T Get<T>(string key)
        {
            if (IsExist(key))
            {
                return _memoryCache.Get<string>(key).Deserialize<T>();
            }
            return default(T);
        }

        public bool IsExist(string key)
        {
            return _memoryCache.TryGetValue(key, out object? _);
        }

        public bool Remove(string key)
        {
            if (IsExist(key))
            {
                _memoryCache.Remove(key);

                return true;
            }

            return false;
        }

        public bool SetOrUpdate(string key, string value)
        {
            string result = _memoryCache.Set<string>(key, value);

            if (result != null)
            {
                return true;
            }
            return false;
        }
    }
}
