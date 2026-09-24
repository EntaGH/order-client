namespace Client.Infrastructure.HttpClients;

public class HttpClientSettings
{
    public string OrderBaseUrl { get; set; } = default!;
    public string InventoryBaseUrl { get; set; } = default!;
}
