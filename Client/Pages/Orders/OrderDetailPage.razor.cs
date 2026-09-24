using Client.Pages.Orders.Models;
using Client.Pages.Orders.Services;
using Microsoft.AspNetCore.Components;
using System.Net;

namespace Client.Pages.Orders;

public partial class OrderDetailPage
{
    [Parameter]
    public Guid Id { get; set; }

    [Inject]
    private IOrderService _orderService { get; set; } = default!;

    private Order? _order { get; set; }

    private string? _message { get; set; }

    private bool _isLoading { get; set; }

    private bool _isRequestingPayment { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadOrderAsync();
    }

    private async Task LoadOrderAsync()
    {
        _isLoading = true;
        _message = null;

        try
        {
            var result = await _orderService.GetByIdAsync(Id);

            if (result.StatusCode != HttpStatusCode.OK)
            {
                _order = null;

                _message = result.ErrorResult?.Message
                    ?? "Failed to load order.";

                return;
            }

            _order = result.SuccessResult?.Data;
            _message = result.SuccessResult?.Message;
        }
        catch (Exception ex)
        {
            _order = null;
            _message = ex.Message;
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task RequestPaymentAsync()
    {
        _isRequestingPayment = true;
        _message = null;

        try
        {
            var result = await _orderService.RequestPaymentAsync(Id);

            if (result.StatusCode != HttpStatusCode.OK)
            {
                _message = result.ErrorResult?.Message
                    ?? "Failed to request payment.";

                return;
            }

            await LoadOrderAsync();
        }
        catch (Exception ex)
        {
            _message = ex.Message;
        }
        finally
        {
            _isRequestingPayment = false;
        }
    }
}