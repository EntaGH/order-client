using Client.Infrastructure.Extensions;
using Client.Pages.Inventories.Models;
using Client.Pages.Inventories.Services;
using Microsoft.AspNetCore.Components;
using System.Net;

namespace Client.Pages.Inventories;

public partial class StockItemsPage
{
    private const int PageSize = 10;

    [Parameter]
    public int Page { get; set; } = 1;

    [Inject]
    private IInventoryService _InventoryService { get; set; } = default!;

    private IEnumerable<StockItem> _stockItems { get; set; } = [];

    private PageInfo? _pageInfo { get; set; }

    private string? _message { get; set; }

    private string _sku { get; set; } = string.Empty;

    private bool _isLoading { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadStockItemsAsync();
    }

    private async Task SearchAsync(string sku)
    {
        _sku = sku;
        Page = 1;

        await LoadStockItemsAsync();
    }

    private async Task NextPageAsync()
    {
        if (_pageInfo is null || !_pageInfo.HasNext)
        {
            return;
        }

        Page++;

        await LoadStockItemsAsync();
    }

    private async Task PreviousPageAsync()
    {
        if (_pageInfo is null || !_pageInfo.HasPrevious)
        {
            return;
        }

        Page--;

        await LoadStockItemsAsync();
    }

    private async Task LoadStockItemsAsync()
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

            var result = await _InventoryService.GetAsync(query, _sku);

            if (result.StatusCode != HttpStatusCode.OK)
            {
                _stockItems = [];
                _pageInfo = null;

                _message = result.ErrorResult?.Message ?? "Failed to load stock items.";

                return;
            }

            var data = result.SuccessResult?.Data;

            _stockItems = data?.PagedData ?? [];
            _pageInfo = data?.PageInfo;
            _message = result.SuccessResult?.Message;
        }
        catch (Exception ex)
        {
            _stockItems = [];
            _pageInfo = null;

            _message = ex.Message;
        }
        finally
        {
            _isLoading = false;
        }
    }
}