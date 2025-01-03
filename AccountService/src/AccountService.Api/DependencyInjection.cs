using AspNetCore.OutboundParameterTransformers;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Text.Json.Serialization;
using AspNetCore.Services;
using Application.Abstractions;
using AspNetCore;
using MassTransit;
using AccountService.Infrastructure.Persistence;

namespace AccountService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddSingleton<ICurrentApplicationUserService, CurrentApplicationUserService>();

        services.AddMassTransitCustom(configuration);

        services.AddAuth(configuration);

        services.AddControllers(options =>
        {
            options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
        })
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddEndpointsApiExplorer();

        services.AddSwagger();

        services.AddCors(opt => opt.AddPolicy("DEV_CORS", p => p
            .AllowAnyOrigin()           // TODO: Change this to API Gateway.
            .AllowAnyMethod()
            .AllowAnyHeader()));

        return services;
    }

    private static IServiceCollection AddMassTransitCustom(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .Configure<RabbitMqTransportOptions>(configuration.GetRequiredSection("RabbitMQ"))
            .AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddEntityFrameworkOutbox<ApplicationDbContext>(o =>
            {
                // configure which database lock provider to use (Postgres, SqlServer, or MySql)
                o.UsePostgres();

                o.QueryDelay = TimeSpan.FromSeconds(1);
                o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);

                // enable the bus outbox
                o.UseBusOutbox();
            });

            x.SetInMemorySagaRepositoryProvider();

            x.AddConsumersFromNamespaceContaining<IApiMarker>();
            x.AddSagaStateMachinesFromNamespaceContaining<IApiMarker>();
            x.AddSagasFromNamespaceContaining<IApiMarker>();
            x.AddActivitiesFromNamespaceContaining<IApiMarker>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });

            x.AddConfigureEndpointsCallback((context, name, cfg) =>
            {
                cfg.UseEntityFrameworkOutbox<ApplicationDbContext>(context);
            });
        });
    }
}
