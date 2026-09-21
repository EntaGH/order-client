using Client.Infrastructure.Extensions;
using Client.Pages.Orders.Models;
using Client.Pages.Orders.Services;
using Microsoft.AspNetCore.Components;
using System.Net;

namespace Client.Pages.Orders;

public partial class OrderPage
{
    private const int PageSize = 10;

    public string? Message { get; set; }
    public IEnumerable<Order>? Orders { get; set; } = [];
    public PageInfo? PageInfo { get; set; }

    [Parameter]
    public int Page { get; set; } = 1;

    [Inject]
    IOrderService _orderService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await GetOrders();
    }

    private async Task GetOrders()
    {
        var queryContainer = new QueryContainer()
        {
            Current = Page,
            PageSize = PageSize,

        };

        var result = await _orderService.GetAsync(queryContainer, "");
        if (result.StatusCode == HttpStatusCode.OK)
        {
            Orders = result.SuccessResult!.Data?.PagedData;
            Message = result.SuccessResult!.Message;
            return;
        }

        Message = result.ErrorResult!.Message;
    }

}
