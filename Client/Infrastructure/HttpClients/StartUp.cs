namespace Client.Infrastructure.HttpClients;

internal static class Startup
{
    public static IServiceCollection UseHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<HttpClientSettings>()
            .BindConfiguration(nameof(HttpClientSettings));

        services.AddScoped<HttpClient>();

        return services;
    }
}