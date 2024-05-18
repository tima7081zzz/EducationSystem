using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DalDependencyInjection
{
    public static IServiceCollection AddDal(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["DbConnection"];
        services.AddDbContext<DataContext>((options) =>
        {
            options.UseSqlServer(connectionString);
        });
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}