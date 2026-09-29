using System;

namespace AppForSEII.API.Models;

[PrimaryKey(nameof(ItemId), nameof(PurchaseId))]
public class PurchaseItem
{
    public PurchaseItem()
    {
    }
    
    public PurchaseItem(int itemId, int purchaseId, int quantity, decimal price)
    {
        ItemId = itemId;
        PurchaseId = purchaseId;
        Quantity = quantity;
        Price = price;
    }

    public int ItemId { get; set; }

    [ForeignKey(nameof(ItemId))]
    public Item Item { get; set; }

    public int PurchaseId { get; set; }

    [ForeignKey(nameof(PurchaseId))]
    public Purchase Purchase { get; set; }

    public int Quantity { get; set; } = 1;

    public decimal Price { get; set; }
}
