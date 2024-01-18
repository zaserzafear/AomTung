using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Net;
using System.Text;

namespace AomTung.DataAccessLayer.Extensions.DistributedCache
{
    internal class DistributedCacheExtensions : IDistributedCacheExtensions
    {
        private readonly DataAccessLayerOptions dataAccessLayerOptions;
        private readonly IDistributedCache distributedCache;

        public DistributedCacheExtensions(IOptions<DataAccessLayerOptions> dataAccessLayerOptions, IDistributedCache distributedCache)
        {
            this.dataAccessLayerOptions = dataAccessLayerOptions.Value;
            this.distributedCache = distributedCache;
        }

        public void ClearAll()
        {
            var db = GetRedisDatabase();
            var keysToDelete = GetAllKeysDistributedCache(db);

            foreach (var key in keysToDelete)
            {
                var keyname = key.ToString();
                db.KeyDelete(keyname);
            }
        }

        public async Task<T?> GetAsync<T>(string key) where T : class
        {
            try
            {
                var bytesPlainText = await distributedCache.GetAsync(key);
                if (bytesPlainText == null)
                {
                    return default;
                }

                var plainText = Encoding.UTF8.GetString(bytesPlainText);
                return System.Text.Json.JsonSerializer.Deserialize<T>(plainText);
            }
            catch
            {
                await distributedCache.RemoveAsync(key);
                return default;
            }
        }

        public async Task SetAsync<T>(string key, T obj, double absoluteExpirationRelativeToNowSeconds, double? slidingExpirationSeconds = null) where T : class
        {
            var plainText = System.Text.Json.JsonSerializer.Serialize(obj);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(absoluteExpirationRelativeToNowSeconds)
            };

            if (slidingExpirationSeconds.HasValue)
            {
                options.SlidingExpiration = TimeSpan.FromSeconds(slidingExpirationSeconds.Value);
            }

            var toWrite = Encoding.UTF8.GetBytes(plainText);
            await distributedCache.SetAsync(key, toWrite, options);
        }

        public async Task<string?> GetStringAsync(string key)
        {
            var bytesPlainText = await distributedCache.GetAsync(key);
            if (bytesPlainText == null)
            {
                return default;
            }

            return Encoding.UTF8.GetString(bytesPlainText);
        }

        public async Task SetStringAsync(string key, string value, double absoluteExpirationRelativeToNowSeconds, double? slidingExpirationSeconds = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(absoluteExpirationRelativeToNowSeconds)
            };

            if (slidingExpirationSeconds.HasValue)
            {
                options.SlidingExpiration = TimeSpan.FromSeconds(slidingExpirationSeconds.Value);
            }

            var toWrite = Encoding.UTF8.GetBytes(value);
            await distributedCache.SetAsync(key, toWrite, options);
        }

        private List<string> GetAllKeysDistributedCache(IDatabase db)
        {
            EndPoint endPoint = db.Multiplexer.GetEndPoints().First();
            var pattern = $"{dataAccessLayerOptions.RedisInstanceName}*";
            var server = db.Multiplexer.GetServer(endPoint);
            List<string> keyValuePairs = new List<string>();
            foreach (var key in server.Keys(pattern: pattern))
            {
                keyValuePairs.Add(key);
            }

            return keyValuePairs;
        }

        private IDatabase GetRedisDatabase()
        {
            ConfigurationOptions options = ConfigurationOptions.Parse(dataAccessLayerOptions.RedisConnectionString);
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(options);
            return connection.GetDatabase();
        }
    }
}
