using AomTung.DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AomTung.DataAccessLayer.Extensions
{
    public static class DataAccessLayerExtension
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, DataAccessLayerOptions options)
        {
            AddDbContext<AomTungDbContext>(services, options.AomTungConnection);
            AddDbContext<AomTungExtendDbContext>(services, options.AomTungConnection);

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
