namespace Client.Pages.Inventories.Models;

public class StockItem
{
    public Guid Id { get; set; }

    public string Sku { get; set; } = default!;

    public int QuantityOnHand { get; set; }

    public int QuantityReserved { get; set; } = 0;

    public DateTimeOffset CreatedAt { get; set; }
}
