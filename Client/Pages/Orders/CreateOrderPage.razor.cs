using Client.Pages.Orders.Models;
using Client.Pages.Orders.Request;
using Client.Pages.Orders.Services;
using Microsoft.AspNetCore.Components;
using System.Net;

namespace Client.Pages.Orders;

public partial class CreateOrderPage
{
    [Inject]
    private IOrderService _orderService { get; set; } = default!;

    [Inject]
    private NavigationManager _navigationManager { get; set; } = default!;

    private bool _isLoading { get; set; }

    private string? _message { get; set; }

    private async Task CreateOrderAsync(CreateOrderRequest request)
    {
        _isLoading = true;
        _message = null;

        try
        {
            var result = await _orderService.CreateAsync(request);

            if (result.StatusCode != HttpStatusCode.Accepted)
            {
                _message = result.ErrorResult?.Message
                    ?? "Failed to create order.";

                return;
            }

            var order = result.SuccessResult?.Data;

            if (order is null)
            {
                _message = "Order was created but no data was returned.";

                return;
            }

            _navigationManager.NavigateTo($"/orders/{order.Id}");
        }
        catch (Exception ex)
        {
            _message = ex.Message;
        }
        finally
        {
            _isLoading = false;
        }
    }
}