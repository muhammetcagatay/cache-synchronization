using Newtonsoft.Json;
using StackExchange.Redis;

namespace memory_cache_synchronization.Helpers
{
    public static class ObjectExtensions
    {
        public static string Serialize(this object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static T? Deserialize<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static T? Deserialize<T>(this RedisValue value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
