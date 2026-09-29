using System;

namespace AppForSEII.API.Models;

public class Purchase
{
    public Purchase()
    {
        PurchaseItems = new List<PurchaseItem>();
    }
    public Purchase(DateTime purchaseDate, string purchaseDescription, double totalPrice, string street, string city, string province, string creditCardNumber, string ccv, string expirationDate, bool wrapped, PurchaseStatus purchaseStatus, string userId)
    {
        PurchaseDate = purchaseDate;
        PurchaseDescription = purchaseDescription;
        TotalPrice = totalPrice;
        Street = street;
        City = city;
        Province = province;
        CreditCardNumber = creditCardNumber;
        CCV = ccv;
        ExpirationDate = expirationDate;
        Wrapped = wrapped;
        PurchaseStatus = purchaseStatus;
        UserId = userId;
        PurchaseItems = new List<PurchaseItem>();
    }
    
    [Key]
    public int Id { get; set; }

    public DateTime PurchaseDate { get; set; }

    public string? PurchaseDescription { get; set; }

    public double TotalPrice { get; set; }

    [StringLength(255, MinimumLength = 3, ErrorMessage = "Between 3 and 255 chars")]
    public string Street { get; set; }

    [StringLength(255, MinimumLength = 3, ErrorMessage = "Between 3 and 255 chars")]
    public string City { get; set; }

    [StringLength(255, MinimumLength = 3, ErrorMessage = "Between 3 and 255 chars")]
    public string Province { get; set; }

    public string CreditCardNumber { get; set; } = "12345678901234567890";

    public string CCV { get; set; } = "123";

    public string ExpirationDate { get; set; } = "10/30";

    public bool Wrapped { get; set; } = false;

    public PurchaseStatus PurchaseStatus { get; set; }

    public string UserId { get; set; } = "";

    public IList<PurchaseItem> PurchaseItems {get; set;}
}
