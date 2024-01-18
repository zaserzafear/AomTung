namespace AomTung.DataAccessLayer.Extensions
{
    public record DataAccessLayerOptions
    {
        public string AomTungDbConnectionString { get; set; } = null!;
        public string RedisInstanceName { get; set; } = null!;
        public string RedisConnectionString { get; set; } = null!;
        public double MasterDataCacheSeconds { get; set; }
    }
}
