namespace AomTung.DataAccessLayer.Extensions.DistributedCache
{
    public interface IDistributedCacheExtensions
    {
        void ClearAll();
        Task SetAsync<T>(string key, T obj, double absoluteExpirationRelativeToNowSeconds, double? slidingExpirationSeconds = null) where T : class;

        Task<T?> GetAsync<T>(string key) where T : class;

        Task SetStringAsync(string key, string value, double absoluteExpirationRelativeToNowSeconds, double? slidingExpirationSeconds = null);

        Task<string?> GetStringAsync(string key);

    }
}
