namespace Client.Pages.Orders.Models;

public class OrderLine
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; } = default!;

    public Order? Order { get; set; }

    public string Sku { get; set; } = default!;

    public int Quantity { get; set; }

    public double UnitPrice { get; set; } 

    public DateTimeOffset CreatedAt { get; set; }
}