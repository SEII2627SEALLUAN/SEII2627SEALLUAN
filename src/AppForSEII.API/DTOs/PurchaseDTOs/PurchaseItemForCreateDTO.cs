public class PurchaseItemForCreateDTO //asks for name and quantity to the user. Correctly passes name to details
{
    [Required]
    public string Name { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }

    public PurchaseItemForCreateDTO(string name, int quantity){
        Name = name;
        Quantity = quantity;
    }
    public bool Equals(object? obj)
    {
        return obj is PurchaseItemForCreateDTO dTO &&
        Name == dTO.Name &&
        Quantity == dTO.Quantity;
    }
}