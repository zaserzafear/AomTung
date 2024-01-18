using AomTung.DataAccessLayer.Extensions;
using AomTung.DataAccessLayer.Extensions.DistributedCache;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AomTung.DataAccessLayer.Data
{
    public class AomTungExtendDbContext : AomTungDbContext
    {
        private readonly IDistributedCacheExtensions _cache;
        private readonly DataAccessLayerOptions _accessLayerOptions;

        public AomTungExtendDbContext(DbContextOptions<AomTungDbContext> options, IDistributedCacheExtensions cache, IOptions<DataAccessLayerOptions> accessLayerOptions) : base(options)
        {
            _cache = cache;
            _accessLayerOptions = accessLayerOptions.Value;
        }

        public async Task<string> GetAesSaltKey()
        {
            string cacheKey = "AesSaltKey";
            var aesSaltKey = await _cache.GetStringAsync(cacheKey);
            if (aesSaltKey != null)
            {
                return aesSaltKey;
            }

            aesSaltKey = Database
                .SqlQuery<string>($"SELECT GetAesSaltKey() AS salt;")
                .AsEnumerable()
                .Single();

            await _cache.SetStringAsync(cacheKey, aesSaltKey, _accessLayerOptions.MasterDataCacheSeconds);

            return aesSaltKey;
        }
    }
}
