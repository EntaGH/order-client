using Microsoft.AspNetCore.Components.Web;

namespace Client.Infrastructure.Serializations;

internal static class StartUp
{
    internal static IServiceCollection UseJsonSerialization(this IServiceCollection services)
    {
        services.AddTransient<IJsonSerializerService, JsonSerializerService>();

        return services;
    }
}
