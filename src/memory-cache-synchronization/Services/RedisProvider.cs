using memory_cache_synchronization.Helpers;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace memory_cache_synchronization.Services
{
    public interface IRedisProvider
    {
        ISubscriber GetSubscriber();
        Task<bool> PublishAsync<T>(string channel, T data, CommandFlags commandFlag);
    }

    public class RedisProvider : IRedisProvider
    {
        private readonly AppSettings _appSettings;
        private readonly ConnectionMultiplexer _connection;
        private readonly ISubscriber _pubSub;

        public RedisProvider(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _connection = ConnectionMultiplexer.Connect(_appSettings.RedisConnectionString);
            _pubSub = _connection.GetSubscriber();
        }

        public ISubscriber GetSubscriber()
        {
            return _connection.GetSubscriber();
        }

        public async Task<bool> PublishAsync<T>(string channel, T data, CommandFlags commandFlag)
        {
            long result = await _pubSub.PublishAsync(channel, data.Serialize(), CommandFlags.FireAndForget);

            return result > 0;
        }
    }
}
