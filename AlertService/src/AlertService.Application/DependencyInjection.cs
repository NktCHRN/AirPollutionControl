﻿using Application.Behaviors;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlertService.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddValidatorsFromAssembly(typeof(IApplicationAssemblyMarker).Assembly)
            .AddMediatR(cfg =>
            {
                cfg
                .RegisterServicesFromAssembly(typeof(IApplicationAssemblyMarker).Assembly)
                .AddOpenBehavior(typeof(ValidationBehavior<,>))
                .AddOpenBehavior(typeof(TransactionalBehavior<,>));
            });
    }
}
