using Client.Pages.Orders.Services;

namespace Client.Pages;

public static class StartUp
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services.AddScoped<IOrderService, OrderService>();
    }
}
