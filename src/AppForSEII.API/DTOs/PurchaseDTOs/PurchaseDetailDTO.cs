namespace AppForSEII.API.DTOs.PurchaseDTOs;
using AppForSEII.API.DTOs.ItemDTOs;
using AppForSEII.API.DTOs.PurchaseDTOs;
public class PurchaseDetailDTO
{
    public string CustomerName { get; set; } //ask. From ApplicatuinUserDTO? Inside the method? 
    public string CustomerSurname { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string Description { get; set; }
    public bool Wrapped { get; set; }
    public double TotalPrice { get; set; }
    public IList<PurchaseItemDTO> PurchaseItems { get; set; }
    public PurchaseDetailDTO(string customerName, string customerSurname, DateTime purchaseDate, string description, bool wrapped, double totalPrice, IList<ItemDTO> purchaseItems)
    {
        CustomerName = customerName;
        CustomerSurname = customerSurname;
        PurchaseDate = purchaseDate;
        Description = description;
        Wrapped = wrapped;
        TotalPrice = totalPrice;
        PurchaseItems = purchaseItems;
    }
    public bool Equals(PurchaseDetailDTO other)
    {
        if (other == null) return false;
        if (CustomerName != other.CustomerName) return false;
        if (CustomerSurname != other.CustomerSurname) return false;
        if (PurchaseDate != other.PurchaseDate) return false;
        if (Description != other.Description) return false;
        if (Wrapped != other.Wrapped) return false;
        if (TotalPrice != other.TotalPrice) return false;
        if (PurchaseItems.Count != other.PurchaseItems.Count) return false;
        for (int i = 0; i < PurchaseItems.Count; i++)
        {
            if (!PurchaseItems[i].Equals(other.PurchaseItems[i])) return false;
        }
        return true;
    }
}