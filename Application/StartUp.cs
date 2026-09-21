using Application.Infrastructure.HealthChecks;
using Application.Infrastructure.Mapping;
using Application.Infrastructure.Messaging;
using Application.Infrastructure.Middleware;
using Application.Infrastructure.OpenAPI;
using Application.Infrastructure.Persistence;
using Application.Infrastructure.Persistence.Contexts;
using Application.Infrastructure.Validations;
using AutoMapper.EquivalencyExpression;
using Domain.Definitions.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Application;

public static class Startup
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddFluentValidation()
            .AddAutoMapper(cfg => cfg.AddCollectionMappers(), typeof(MappingProfile))
            .AddHealthCheck(configuration)
            .AddOpenApiDocumentation(configuration)
            .AddPersistence()
            .AddPulsar(configuration)
            .AddServices();

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app, IConfiguration configuration)
    {
        app
            .UseStaticFiles()
            .UseRouting()
            .UseExceptionHandlerMiddleware()
            .UseHealthCheck()
            .UseOpenApiDocumentation();

        return app;
    }

    internal static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .Scan(scan => scan
            .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(filter => filter.AssignableTo<ITransientService>())
                .AsImplementedInterfaces()
                .WithTransientLifetime()
            .AddClasses(filter => filter.AssignableTo<IScopedService>())
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );

        return services;
    }
}