using memory_cache_synchronization.Helpers;
using StackExchange.Redis;

namespace memory_cache_synchronization.Services
{
    public interface ICacheSynchronizeProvider
    {
        public Task SubscribeCacheSyncChannel();
        Task PublishCacheSyncMessage(string key, object value, CacheOperationType cacheOperationType);
    }
    public class CacheSynchronizeProvider : ICacheSynchronizeProvider
    {
        private readonly ICacheProvider _inMemoryCacheProvider;
        private readonly IRedisProvider _redisProvider;
        private readonly ISubscriber _pubSub;
        private readonly string _channel = "CACHESYNC";

        public CacheSynchronizeProvider(IRedisProvider redisProvider, ICacheProvider inMemoryCacheProvider)
        {
            _redisProvider = redisProvider;
            _pubSub = _redisProvider.GetSubscriber();
            _inMemoryCacheProvider = inMemoryCacheProvider;
        }

        public async Task SubscribeCacheSyncChannel()
        {
            await _pubSub.SubscribeAsync(_channel, (channel, message) =>
            {
                CacheSyncMessage syncMessage = message.Deserialize<CacheSyncMessage>();

                HandleCacheSyncMessage(syncMessage);
            });
        }

        public async Task PublishCacheSyncMessage(string key, object value, CacheOperationType cacheOperationType)
        {
            CacheSyncMessage cacheSyncMessage = new CacheSyncMessage
            {
                CacheOperationType = cacheOperationType,
                Key = key,
                Value = value.Serialize(),
            };

            await _pubSub.PublishAsync(_channel, cacheSyncMessage.Serialize());
        }

        private void HandleCacheSyncMessage(CacheSyncMessage syncMessage)
        {
            switch (syncMessage.CacheOperationType)
            {
                case CacheOperationType.SETORUPDATE:
                    _inMemoryCacheProvider.SetOrUpdate(syncMessage.Key, syncMessage.Value);
                    break;
                case CacheOperationType.REMOVE:
                    _inMemoryCacheProvider.Remove(syncMessage.Key);
                    break;
                default:
                    break;
            }
        }
    }
}
