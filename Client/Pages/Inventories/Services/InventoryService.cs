using Client.Infrastructure.Extensions;
using Client.Infrastructure.HttpClients;
using Client.Pages.Inventories.Models;
using Client.Pages.Inventories.Requests;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;

namespace Client.Pages.Inventories.Services;

public interface IInventoryService
{
    Task<ResultWrapper<PaginationResponse<StockItem>>> GetAsync(QueryContainer query, string? sku);

    Task<ResultWrapper<StockItem>> CreateAsync(CreateStockItemRequest request);

    Task<ResultWrapper<StockItem>> UpdateAsync(Guid id, UpdateStockItemRequest request);

    Task<ResultWrapper<StockItem>> GetByIdAsync(Guid id);
}

public class InventoryService : IInventoryService
{
    private readonly string _getUrl = "{0}StockItems?Sku={1}&Current={2}&PageSize={3}";
    private readonly string _postUrl = "{0}StockItems";
    private readonly string _putUrl = "{0}StockItems/{1}";
    private readonly string _getDetailUrl = "{0}StockItems/{1}";
    private readonly HttpClient _httpClient;
    private readonly HttpClientSettings _httpClientSettings;

    public InventoryService(HttpClient httpClient, IOptions<HttpClientSettings> httpClientSettings)
    {
        _httpClient = httpClient;
        _httpClientSettings = httpClientSettings.Value;
    }

    public async Task<ResultWrapper<PaginationResponse<StockItem>>> GetAsync(QueryContainer query, string? sku)
    {
        var url = string.Format(
            _getUrl,
            _httpClientSettings.InventoryBaseUrl,
            sku ?? string.Empty,
            query.Current,
            query.PageSize);

        var response = await _httpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return new ResultWrapper<PaginationResponse<StockItem>>()
            {
                StatusCode = HttpStatusCode.OK,
                SuccessResult = await response.Content
                    .ReadFromJsonAsync<
                        SuccessResultWrapper<PaginationResponse<StockItem>>>()
            };
        }

        return new ResultWrapper<PaginationResponse<StockItem>>()
        {
            StatusCode = response.StatusCode,
            ErrorResult = await response.Content
                .ReadFromJsonAsync<ErrorResultWrapper>()
        };
    }

    public async Task<ResultWrapper<StockItem>> CreateAsync(CreateStockItemRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(
            string.Format(
                _postUrl,
                _httpClientSettings.InventoryBaseUrl),
            request);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return new ResultWrapper<StockItem>
            {
                StatusCode = response.StatusCode,
                SuccessResult = await response.Content.ReadFromJsonAsync<SuccessResultWrapper<StockItem>>()
            };
        }

        return new ResultWrapper<StockItem>
        {
            StatusCode = response.StatusCode,
            ErrorResult = await response.Content.ReadFromJsonAsync<ErrorResultWrapper>()
        };
    }

    public async Task<ResultWrapper<StockItem>> UpdateAsync(Guid id, UpdateStockItemRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync(
            string.Format(
                _putUrl,
                _httpClientSettings.InventoryBaseUrl,
                id),
            request);

        if (response.IsSuccessStatusCode)
        {
            return new ResultWrapper<StockItem>
            {
                StatusCode = response.StatusCode,
                SuccessResult = await response.Content
                    .ReadFromJsonAsync<SuccessResultWrapper<StockItem>>()
            };
        }

        return new ResultWrapper<StockItem>
        {
            StatusCode = response.StatusCode,
            ErrorResult = await response.Content
                .ReadFromJsonAsync<ErrorResultWrapper>()
        };
    }

    public async Task<ResultWrapper<StockItem>> GetByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync(
            string.Format(
                _getDetailUrl,
                _httpClientSettings.InventoryBaseUrl,
                id));

        if (response.IsSuccessStatusCode)
        {
            return new ResultWrapper<StockItem>
            {
                StatusCode = response.StatusCode,
                SuccessResult = await response.Content
                    .ReadFromJsonAsync<SuccessResultWrapper<StockItem>>()
            };
        }

        return new ResultWrapper<StockItem>
        {
            StatusCode = response.StatusCode,
            ErrorResult = await response.Content
                .ReadFromJsonAsync<ErrorResultWrapper>()
        };
    }
}
