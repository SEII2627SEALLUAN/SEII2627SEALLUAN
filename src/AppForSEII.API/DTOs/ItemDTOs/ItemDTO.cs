namespace AppForSEII.API.DTOs.ItemDTOs;

public class ItemDTO
{
    public string Name { get; set; }
    public int? RecommendedAge { get; set; }
    public Gender? Gender { get; set; }
    public decimal Price { get; set; }
    public string Size { get; set; }
    public int QuantityAvailableForPurchase { get; set; }
    public string SportName { get; set; }
    public ItemDTO(string name, int? recommendedAge, Gender? gender, decimal price, string size, int quantityAvailableForPurchase, string sportName)
    {
        Name = name;
        RecommendedAge = recommendedAge;
        Gender = gender;
        Price = price;
        Size = size;
        QuantityAvailableForPurchase = quantityAvailableForPurchase;
        SportName = sportName;
    }

    public bool Equals(ItemDTO other)
    {
        if (other == null) return false;
        if (Name != other.Name) return false;
        if (RecommendedAge != other.RecommendedAge) return false;
        if (Gender != other.Gender) return false;
        if (Price != other.Price) return false;
        if (Size != other.Size) return false;
        if (SportName != other.SportName) return false;
        return true;
    }
}
