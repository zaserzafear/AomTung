using AomTung.Application.Member;
using AomTung.DataAccessLayer.Extensions;
using AomTung.DataAccessLayer.Repositories;
using AomTung.Domain.Member.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AomTung.Application.Extensions
{
    public static class ApplicationExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataAccessLayer(configuration);

            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IMemberService, MemberService>();

            return services;
        }
    }
}
