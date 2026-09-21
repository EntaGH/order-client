using Client.Infrastructure.Extensions;
using Client.Infrastructure.HttpClients;
using Client.Pages.Orders.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;

namespace Client.Pages.Orders.Services;

public interface IOrderService
{
    Task<ResultWrapper<PaginationResponse<Order>>> GetAsync(QueryContainer query, string? customerId);
}

public class OrderService : IOrderService
{
    private const string Endpoint = "Orders";
    private const string CustomerId = "CustomerId";
    private const string PageSize = "PageSize";
    private const string Current = "Current";

    private readonly HttpClientSettings _httpClientSettings;
    private readonly HttpClient _httpClient;

    public OrderService(IOptions<HttpClientSettings> httpClientSettings, HttpClient httpClient)
    {
        _httpClientSettings = httpClientSettings.Value;
        _httpClient = httpClient;
    }

    public async Task<ResultWrapper<PaginationResponse<Order>>> GetAsync(QueryContainer query, string? customerId)
    {
        var url = QueryBuilder(query, customerId is null ? string.Empty : customerId);

        var response = await _httpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return new ResultWrapper<PaginationResponse<Order>>()
            {
                StatusCode = HttpStatusCode.OK,
                SuccessResult = await response.Content.ReadFromJsonAsync<SuccessResultWrapper<PaginationResponse<Order>>>()
            }
            ;
        }

        return new ResultWrapper<PaginationResponse<Order>>()
        {
            StatusCode = response.StatusCode,
            ErrorResult = await response.Content.ReadFromJsonAsync<ErrorResultWrapper>()
        };
    }

    private string QueryBuilder(QueryContainer query, string? customerId)
    {
        return
            $"{_httpClientSettings.ApiBaseUrl}{Endpoint}?" +
            $"{CustomerId}={customerId}&" +
            $"{PageSize}={query.PageSize}&" +
            $"{Current}={query.Current}";
    }
}
