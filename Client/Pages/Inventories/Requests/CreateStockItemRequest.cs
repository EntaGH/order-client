namespace Client.Pages.Inventories.Requests;

public class CreateStockItemRequest
{
    public string Sku { get; set; } = string.Empty;

    public int QuantityOnHand { get; set; }
}