using Api.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Abstractions.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterDataProviderService(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IDataProviderService, DataProviderService>();
            return services;
        }
    }
}
