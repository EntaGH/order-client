using Client.Infrastructure.Extensions;
using Client.Pages.Orders.Models;
using Client.Pages.Orders.Services;
using Microsoft.AspNetCore.Components;
using System.Net;

namespace Client.Pages.Orders;

public partial class OrderPage
{
    private const int PageSize = 10;

    [Parameter]
    public int Page { get; set; } = 1;

    [Parameter]
    public string CustomerId { get; set; } = string.Empty;

    [Inject]
    private IOrderService _orderService { get; set; } = default!;

    private IEnumerable<Order> _orders { get; set; } = [];

    private PageInfo? _pageInfo { get; set; }

    private string? _message { get; set; }

    private bool _isLoading { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadOrdersAsync();
    }

    private async Task OnSearchChangedAsync(string customerId)
    {
        CustomerId = customerId;
        Page = 1;

        await LoadOrdersAsync();
    }

    private async Task NextPageAsync()
    {
        if (_pageInfo is null || !_pageInfo.HasNext)
        {
            return;
        }

        Page++;

        await LoadOrdersAsync();
    }

    private async Task PreviousPageAsync()
    {
        if (_pageInfo is null || !_pageInfo.HasPrevious)
        {
            return;
        }

        Page--;

        await LoadOrdersAsync();
    }

    private async Task LoadOrdersAsync()
    {
        _isLoading = true;
        _message = null;

        try
        {
            var query = new QueryContainer
            {
                Current = Page,
                PageSize = PageSize
            };

            var result = await _orderService.GetAsync(
                query,
                CustomerId);

            if (result.StatusCode != HttpStatusCode.OK)
            {
                _orders = [];
                _pageInfo = null;

                _message = result.ErrorResult?.Message ?? "Failed to load orders.";

                return;
            }

            var data = result.SuccessResult?.Data;

            _orders = data?.PagedData ?? [];
            _pageInfo = data?.PageInfo;
            _message = result.SuccessResult?.Message;
        }
        catch (Exception ex)
        {
            _orders = [];
            _pageInfo = null;
            _message = ex.Message;
        }
        finally
        {
            _isLoading = false;
        }
    }
}