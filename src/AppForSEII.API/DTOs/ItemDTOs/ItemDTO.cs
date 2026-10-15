namespace AppForSEII.API.DTOs.ItemDTOs;

public class ItemDTO
{
    public string Name { get; set; }
    public int RecommendedAge { get; set; }
    public Gender Gender { get; set; }
    public decimal Price { get; set; }
    public string Size { get; set; }
    public Sport Sport { get; set; }
    public ItemDTO(string name, int recommendedAge, Gender gender, decimal price, string size, Sport sport)
    {
        Name = name;
        RecommendedAge = recommendedAge;
        Gender = gender;
        Price = price;
        Size = size;
        Sport = sport;
    }
}
