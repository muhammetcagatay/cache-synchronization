using memory_cache_synchronization.Helpers;
using memory_cache_synchronization.Models;

namespace memory_cache_synchronization.Services
{
    public interface IBalanceService
    {
        void CreateOrUpdateBalance(CustomerBalance customerBalance);
        CustomerBalance? GetBalance(int balanceId);
        void DeleteBalance(int balanceId);    
    }
    public class BalanceService : IBalanceService
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly ICacheSynchronizeProvider _cacheSyncProvider;

        public BalanceService(ICacheProvider cacheProvider, ICacheSynchronizeProvider cacheSyncProvider)
        {
            _cacheProvider = cacheProvider;
            _cacheSyncProvider = cacheSyncProvider;
        }

        public void CreateOrUpdateBalance(CustomerBalance customerBalance)
        {
            _cacheSyncProvider.PublishCacheSyncMessage(customerBalance.Id.ToString(), customerBalance,CacheOperationType.SETORUPDATE);
        }

        public void DeleteBalance(int balanceId)
        {
            _cacheSyncProvider.PublishCacheSyncMessage(balanceId.ToString(),null,CacheOperationType.REMOVE);
        }

        public CustomerBalance GetBalance(int balanceId)
        {
            return _cacheProvider.Get<CustomerBalance>(balanceId.ToString());
        }
    }
}
