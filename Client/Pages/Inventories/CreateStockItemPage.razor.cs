using Client.Pages.Inventories.Models;
using Client.Pages.Inventories.Requests;
using Client.Pages.Inventories.Services;
using Microsoft.AspNetCore.Components;
using System.Net;

namespace Client.Pages.Inventories;

public partial class CreateStockItemPage
{
    [Inject]
    private IInventoryService _stockItemService { get; set; } = default!;

    [Inject]
    private NavigationManager _navigationManager { get; set; } = default!;

    private CreateStockItemRequest _request { get; set; } = new();

    private string? _message { get; set; }

    private bool _isLoading { get; set; }

    private async Task CreateAsync()
    {
        _isLoading = true;
        _message = null;

        try
        {
            var result = await _stockItemService.CreateAsync(_request);

            if (result.StatusCode != HttpStatusCode.OK)
            {
                _message = result.ErrorResult?.Message ?? "Failed to create stock item.";

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