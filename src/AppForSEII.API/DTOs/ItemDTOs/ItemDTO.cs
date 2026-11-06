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

    public override bool Equals(object? obj) => Equals(obj as ItemDTO);

    public bool Equals(ItemDTO? other)
    {
        if (other is null) return false;
        return Name == other.Name
            && RecommendedAge == other.RecommendedAge
            && Gender == other.Gender
            && Price == other.Price
            && Size == other.Size
            && QuantityAvailableForPurchase == other.QuantityAvailableForPurchase
            && SportName == other.SportName;
    }

    public override int GetHashCode() =>
        HashCode.Combine(Name, RecommendedAge, Gender, Price, Size,
                         QuantityAvailableForPurchase, SportName);

}
