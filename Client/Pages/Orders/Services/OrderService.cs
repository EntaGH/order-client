using Client.Infrastructure.Extensions;
using Client.Infrastructure.HttpClients;
using Client.Pages.Orders.Models;
using Client.Pages.Orders.Request;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;

namespace Client.Pages.Orders.Services;

public interface IOrderService
{
    Task<ResultWrapper<PaginationResponse<Order>>> GetAsync(QueryContainer query, string? customerId);

    Task<ResultWrapper<Order>> GetByIdAsync(Guid id);

    Task<ResultWrapper<Order>> CreateAsync(CreateOrderRequest request);

    Task<ResultWrapper<object>> RequestPaymentAsync(Guid id);
}

public class OrderService : IOrderService
{
    private const string Endpoint = "Orders";

    private const string RequestPayment = "RequestPayment";
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

    public async Task<ResultWrapper<Order>> GetByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"{_httpClientSettings.OrderBaseUrl}{Endpoint}/{id}");

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return new ResultWrapper<Order>()
            {
                StatusCode = HttpStatusCode.OK,
                SuccessResult = await response.Content
                    .ReadFromJsonAsync<SuccessResultWrapper<Order>>()
            };
        }

        return new ResultWrapper<Order>()
        {
            StatusCode = response.StatusCode,
            ErrorResult = await response.Content.ReadFromJsonAsync<ErrorResultWrapper>()
        };
    }

    public async Task<ResultWrapper<Order>> CreateAsync(CreateOrderRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{_httpClientSettings.OrderBaseUrl}{Endpoint}",
            request);

        if (response.StatusCode == HttpStatusCode.Accepted)
        {
            return new ResultWrapper<Order>()
            {
                StatusCode = response.StatusCode,
                SuccessResult = await response.Content
                    .ReadFromJsonAsync<SuccessResultWrapper<Order>>()
            };
        }

        return new ResultWrapper<Order>()
        {
            StatusCode = response.StatusCode,
            ErrorResult = await response.Content
                .ReadFromJsonAsync<ErrorResultWrapper>()
        };
    }

    public async Task<ResultWrapper<object>> RequestPaymentAsync(Guid id)
    {
        var response = await _httpClient.PostAsync(
            $"{_httpClientSettings.OrderBaseUrl}{Endpoint}/{id}/RequestPayment",
            null);

        if (response.IsSuccessStatusCode)
        {
            return new ResultWrapper<object>()
            {
                StatusCode = response.StatusCode
            };
        }

        return new ResultWrapper<object>()
        {
            StatusCode = response.StatusCode,
            ErrorResult = await response.Content
                .ReadFromJsonAsync<ErrorResultWrapper>()
        };
    }

    private string QueryBuilder(QueryContainer query, string? customerId)
    {
        return
            $"{_httpClientSettings.OrderBaseUrl}{Endpoint}?" +
            $"{CustomerId}={customerId}&" +
            $"{PageSize}={query.PageSize}&" +
            $"{Current}={query.Current}";
    }
}
