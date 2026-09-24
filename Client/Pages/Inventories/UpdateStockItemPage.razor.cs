using Client.Pages.Inventories.Models;
using Client.Pages.Inventories.Requests;
using Client.Pages.Inventories.Services;
using Microsoft.AspNetCore.Components;
using System.Net;

namespace Client.Pages.Inventories;

public partial class UpdateStockItemPage
{
    [Parameter]
    public Guid Id { get; set; }

    [Inject]
    private IInventoryService _stockItemService { get; set; } = default!;

    [Inject]
    private NavigationManager _navigationManager { get; set; } = default!;

    private StockItem? _stockItem { get; set; }

    private UpdateStockItemRequest _request { get; set; } = new();

    private string? _message { get; set; }

    private bool _isLoading { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadStockItemAsync();
    }

    private async Task LoadStockItemAsync()
    {
        _isLoading = true;
        _message = null;

        try
        {
            var result = await _stockItemService.GetByIdAsync(Id);

            if (result.StatusCode != HttpStatusCode.OK)
            {
                _message = result.ErrorResult?.Message
                    ?? "Failed to load stock item.";

                return;
            }

            _stockItem = result.SuccessResult?.Data;

            if (_stockItem is null)
            {
                _message = "Stock item was not found.";

                return;
            }

            _request.QuantityOnHand = _stockItem.QuantityOnHand;
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

    private async Task UpdateAsync()
    {
        _isLoading = true;
        _message = null;

        try
        {
            var result = await _stockItemService.UpdateAsync(
                Id,
                _request);

            if (result.StatusCode != HttpStatusCode.OK)
            {
                _message = result.ErrorResult?.Message
                    ?? "Failed to update stock item.";

                return;
            }

            _navigationManager.NavigateTo("/stock-items");
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