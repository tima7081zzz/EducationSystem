using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Emailing.DI;

public static class AddEmailingDependencyInjection
{
    public static IServiceCollection AddEmailing(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailingOptions>(configuration.GetSection(EmailingOptions.SectionName));
        
        services.AddTransient<IEmailSender, EmailSender>();
        
        return services;
    }
}