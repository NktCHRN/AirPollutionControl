using AlertService.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AlertService.Domain.Abstractions;
using Database.Interceptors;
using System.Text.Json.Serialization;
using System.Text.Json;
using Application.Abstractions;
using AlertService.Infrastructure.Repositories;
using Database;
using AlertService.Infrastructure.Abstractions;
using AlertService.Infrastructure.Seeders;

namespace AlertService.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDatabase(configuration)
            .AddSingleton(TimeProvider.System)
            .Configure((Action<JsonSerializerOptions>)(opt => opt.Converters.Add(new JsonStringEnumConverter())));
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        return services
                    .AddScoped<SoftDeleteInterceptor>()
                    .AddScoped<DispatchDomainEventsInterceptor>()
                                        .AddTransient<ICountrySeeder, CountrySeeder>()
                                                            .AddTransient<IAgglomerationSeeder, AgglomerationSeeder>()
                    .AddDbContext<ApplicationDbContext>((sp, options)
                        => options
                            .UseNpgsql(configuration.GetConnectionString("ApplicationDbConnection"))
                            .AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>(), sp.GetRequiredService<DispatchDomainEventsInterceptor>())
                            .UseSeeding((dbContext, _) =>
                            {
                                var applicationDbContext = (ApplicationDbContext)dbContext;
                                var countrySeeder = sp.GetRequiredService<ICountrySeeder>();
                                countrySeeder.Seed(applicationDbContext);

                                var agglomerationSeeder = sp.GetRequiredService<IAgglomerationSeeder>();
                                agglomerationSeeder.Seed(applicationDbContext);
                            })
                            .UseAsyncSeeding(async (dbContext, _, ct) =>
                            {
                                var applicationDbContext = (ApplicationDbContext)dbContext;
                                var countrySeeder = sp.GetRequiredService<ICountrySeeder>();
                                await countrySeeder.SeedAsync(applicationDbContext, ct);

                                var agglomerationSeeder = sp.GetRequiredService<IAgglomerationSeeder>();
                                await agglomerationSeeder.SeedAsync(applicationDbContext, ct);

                            }))

                    .AddScoped<ITransactionHandler, TransactionHandler>()
                    .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                    .AddScoped<DbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
    }
}
