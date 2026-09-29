using System;

namespace AppForSEII.API.Models;
[Index(nameof(Name), IsUnique = true)]

public class Item
{
    public Item()
    {
        PurchaseItems = new List<PurchaseItem>();
    }
    
    public Item(string name, decimal price, int? recommendedAge, Gender? gender, string? size, int quantityAvailableForPurchase)
    {
        Name = name;
        Price = price;
        RecommendedAge = recommendedAge;
        Gender = gender;
        Size = size;
        QuantityAvailableForPurchase = quantityAvailableForPurchase;
        PurchaseItems = new List<PurchaseItem>();
    }

    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public decimal Price { get; set; }

    public int? RecommendedAge { get; set; }

    public Gender? Gender { get; set; }

    public string? Size { get; set; }

    public int QuantityAvailableForPurchase { get; set; }

    public IList<PurchaseItem> PurchaseItems {get; set;}

    public Sport? Sport { get; set; }
}