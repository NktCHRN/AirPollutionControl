using Microsoft.Extensions.DependencyInjection;

namespace Database;
public static class DependencyInjection
{
    public static IServiceCollection AddTransactionHandler(this IServiceCollection services)
    {
        return services.AddScoped<TransactionHandler>();
    }
}
