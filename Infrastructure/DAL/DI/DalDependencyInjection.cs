using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DalDependencyInjection
{
    public static IServiceCollection AddDal(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DbOptions>(configuration.GetSection(DbOptions.SectionName));
        
        services.AddDbContext<DataContext>((provider, options) =>
        {
            var dbOptions = provider.GetService<IOptionsSnapshot<DbOptions>>();
            options.UseSqlServer(dbOptions!.Value.ConnectionString);
        });
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}