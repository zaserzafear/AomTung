using AomTung.DataAccessLayer.Data;
using AomTung.DataAccessLayer.Extensions.DistributedCache;
using AomTung.DataAccessLayer.Extensions.MySql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AomTung.DataAccessLayer.Extensions
{
    public static class DataAccessLayerExtension
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            var dataAccessLayerOptions = new DataAccessLayerOptions();

            configuration.GetSection("DataAccessLayer").Bind(dataAccessLayerOptions);
            services.Configure<DataAccessLayerOptions>(options => configuration.GetSection("DataAccessLayer").Bind(options));

            AddDbContext<AomTungDbContext>(services, dataAccessLayerOptions.AomTungDbConnectionString);
            AddDbContext<AomTungExtendDbContext>(services, dataAccessLayerOptions.AomTungDbConnectionString);

            services.AddTransient<IMySqlHelper, MySqlHelper>();
            services.AddTransient<IDistributedCacheExtensions, DistributedCacheExtensions>();

            services.AddStackExchangeRedisCache(opt =>
            {
                opt.InstanceName = dataAccessLayerOptions.RedisInstanceName;
                opt.Configuration = dataAccessLayerOptions.RedisConnectionString;
            });

            return services;
        }

        private static void AddDbContext<TContext>(IServiceCollection services, string connectionString) where TContext : DbContext
        {
            services.AddDbContext<TContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
        }
    }
}
