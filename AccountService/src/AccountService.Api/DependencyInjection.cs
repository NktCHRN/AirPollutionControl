using AspNetCore.OutboundParameterTransformers;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Text.Json.Serialization;
using AspNetCore.Services;
using Application.Abstractions;
using AspNetCore;

namespace AccountService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddSingleton<ICurrentApplicationUserService, CurrentApplicationUserService>();

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
}
