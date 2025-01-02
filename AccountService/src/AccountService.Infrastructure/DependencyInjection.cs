using AccountService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AccountService.Domain.Abstractions;
using Database.Interceptors;
using System.Text.Json.Serialization;
using System.Text.Json;
using Application.Abstractions;
using AccountService.Infrastructure.Repositories;
using Database;
using AccountService.Infrastructure.Abstractions;
using AccountService.Infrastructure.Seeders;
using AccountService.Infrastructure.TokensProviders;
using AccountService.Application.Abstractions;

namespace AccountService.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDatabase(configuration)
            .AddIdentityServices(configuration)
            .AddSingleton(TimeProvider.System)
            .Configure((Action<JsonSerializerOptions>)(opt => opt.Converters.Add(new JsonStringEnumConverter())));
    }

    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<Domain.Models.User, IdentityRole<Guid>>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;

            //options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
            //options.Tokens.PasswordResetTokenProvider = "CustomPasswordReset";
        })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<JwtBearerConfigOptions>(configuration.GetRequiredSection("JwtBearer"));
        services.AddSingleton<IJwtTokenProvider, JwtTokenProvider>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        return services
                    .AddScoped<SoftDeleteInterceptor>()
                    .AddScoped<DispatchDomainEventsInterceptor>()
                    .AddTransient<IRoleSeeder, RoleSeeder>()
                    .AddDbContext<ApplicationDbContext>((sp, options)
                        => options
                            .UseNpgsql(configuration.GetConnectionString("ApplicationDbConnection"))
                            .AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>(), sp.GetRequiredService<DispatchDomainEventsInterceptor>())
                            .UseSeeding((dbContext, _) =>
                            {
                                var roleManager = sp.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                                var seeder = sp.GetRequiredService<IRoleSeeder>();
                                seeder.Seed(roleManager);
                            })
                            .UseAsyncSeeding(async (dbContext, _, ct) =>
                            {
                                var roleManager = sp.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                                var seeder = sp.GetRequiredService<IRoleSeeder>();
                                await seeder.SeedAsync(roleManager, ct);
                            }))

                    .AddScoped<ITransactionHandler, TransactionHandler>()
                    .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                    .AddScoped<DbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
    }
}
