using System;

namespace AppForSEII.API.Models;

[PrimaryKey(nameof(ItemId), nameof(PurchaseId))]
public class PurchaseItem
{
    public int ItemId { get; set; }

    [ForeignKey(nameof(ItemId))]
    public Item Item { get; set; }

    public int PurchaseId { get; set; }

    [ForeignKey(nameof(PurchaseId))]
    public Purchase Purchase { get; set; }

    public int Quantity { get; set; } = 1;

    public decimal Price { get; set; }
}
