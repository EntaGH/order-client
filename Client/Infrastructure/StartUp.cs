using Client.Infrastructure.HttpClients;
using Client.Infrastructure.Serializations;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Client.Infrastructure;

internal static class StartUp
{
    public static IServiceCollection UseInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .UseHttpClients(configuration)
            .UseJsonSerialization();

        return services;
    }

    internal static WebAssemblyHostBuilder AddConfigurations(this WebAssemblyHostBuilder builder)
    {
        builder.Configuration
                .AddJsonFiles("appsettings");

        return builder;
    }

    private static IConfigurationBuilder AddJsonFiles(this IConfigurationBuilder builder, string fileName)
    {
        return builder
            .AddJsonFile($"{fileName}.json", optional: false, reloadOnChange: true);
    }
}
