using Client.Pages.Inventories.Services;
using Client.Pages.Orders.Services;

namespace Client.Pages;

public static class StartUp
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IInventoryService, InventoryService>();

        return services;
    }
}
