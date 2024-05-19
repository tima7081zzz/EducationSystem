using Auth.Services;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class AddAuthDependencyInjection
{
    public static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddScoped<ILoginService, LoginService>();

        return services;
    }
}