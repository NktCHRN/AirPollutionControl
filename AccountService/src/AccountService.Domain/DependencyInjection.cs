using Microsoft.Extensions.DependencyInjection;

namespace AccountService.Domain;
public static class DependencyInjection
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        return services;
    }
}
