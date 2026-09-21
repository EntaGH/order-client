namespace Client.Pages.Orders.Models;

public class Order
{
    public Guid Id { get; set; }

    public string CustomerId { get; set; } = default!;

    public double Total { get; set; }

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public ICollection<OrderLine>? OrderLines { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}

public enum OrderStatus
{
    Pending = 1,
    Reserving = 2,
    Charging = 3,
    Confirmed = 4,
    Cancelled = 5,
}