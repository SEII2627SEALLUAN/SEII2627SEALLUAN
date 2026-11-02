namespace AppForSEII.API.DTOS.PurchaseDTOS
{
    public class PurchaseItemDTO //shows items like this in the details of a purchase. Many details indeed
    {
        public string Name { get; set; }
        public int? RecommendedAge { get; set; }
        public Gender? Gender { get; set; }
        public decimal Price { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
        public PurchaseItemDTO(string name, int? recommendedAge, Gender? gender, decimal price, string size, int quantity)
        {
            Name = name;
            RecommendedAge = recommendedAge;
            Gender = gender;
            Price = price;
            Size = size;
            Quantity = quantity;
        }

        public bool Equals(PurchaseItemDTO other)
        {
            if (other == null) return false;
            if (Name != other.Name) return false;
            if (RecommendedAge != other.RecommendedAge) return false;
            if (Gender != other.Gender) return false;
            if (Price != other.Price) return false;
            if (Size != other.Size) return false;
            return true;
        }
    }
}