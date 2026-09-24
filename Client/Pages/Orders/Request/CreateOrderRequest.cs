namespace Client.Pages.Orders.Request;

public class CreateOrderRequest
{
    public string CustomerId { get; set; } = string.Empty;

    public List<CreateOrderLineRequest> OrderLines { get; set; } = [];
}

public class CreateOrderLineRequest
{
    public string Sku { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }
}
