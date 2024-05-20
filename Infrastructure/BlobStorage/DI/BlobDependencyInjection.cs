using BlobStorage.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlobStorage.DI;

public static class BlobDependencyInjection
{
    public static IServiceCollection AddBlob(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BlobOptions>(configuration.GetSection(BlobOptions.SectionName));
        
        services.AddTransient<IBlobStorageManager, BlobStorageManager>();

        return services;
    }
}