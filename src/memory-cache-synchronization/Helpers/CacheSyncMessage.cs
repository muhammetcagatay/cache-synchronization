using System.Text.Json;

namespace memory_cache_synchronization.Helpers
{
    public class CacheSyncMessage
    {
        public CacheOperationType CacheOperationType { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}