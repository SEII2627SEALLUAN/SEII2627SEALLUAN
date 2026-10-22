using AppForSEII.API.DTOS.PurchaseDTOS;

namespace AppForSEII.API.DTOs.PurchaseDTOs
{
    public class PurchaseForCreateDTO
    {
        [Required]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Between 3 and 255 chars")]
        public string Street { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Between 3 and 255 chars")]
        public string City { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Between 3 and 255 chars")]
        public string Province { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 20, ErrorMessage = "Credit card number must be 20 digits")]
        public string CreditCardNumber { get; set; } = "1234-6789-1234-6789";
        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "CCV must be 3 digits")]
        public string CCV { get; set; } = "123";
        [Required]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Expiration date must be in MM/YY format")]
        public string ExpirationDate { get; set; } = "10/30";
        [EmailAddress]
        [Required]
        public string CustomerUserName { get; set; }="peter.jackson@uclm.es";

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your Name and Surname")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Name and Surname must have at least 10 characters")]
        public string CustomerNameSurname { get; set; }="Peter Jackson";
        
        public string? PurchaseDescription { get; set; } = "Purchase";
        public bool Wrapped { get; set; }=false;
        public IList<PurchaseItemDTO> PurchaseItems;

        public PurchaseForCreateDTO(string street, string city, string province, string creditCardNumber, string ccv, string expirationDate, string customerUserName, string customerNameSurname, IList<PurchaseItemDTO> purchaseItemDTOs, string purchaseDescription, bool wrapped)
        {
         Street = street;
         City = city;
         Province = province;
         CreditCardNumber = creditCardNumber;
         CCV = ccv;
         ExpirationDate = expirationDate;
         CustomerUserName = customerUserName;
         CustomerNameSurname = customerNameSurname;
         PurchaseItems = purchaseItemDTOs;
         PurchaseDescription = purchaseDescription;
         Wrapped = wrapped;
        }

        public PurchaseForCreateDTO()
        {
            PurchaseItems = new List<PurchaseItemDTO>();
        }
        public override bool Equals(object? obj)
        {
            return obj is PurchaseForCreateDTO dTO &&
                CustomerUserName == dTO.CustomerUserName &&
                CustomerNameSurname == dTO.CustomerNameSurname &&
                Street == dTO.Street &&
                City == dTO.City &&
                Province == dTO.Province &&
                CreditCardNumber == dTO.CreditCardNumber &&
                CCV == dTO.CCV &&
                ExpirationDate == dTO.ExpirationDate &&
                PurchaseDescription == dTO.PurchaseDescription &&
                Wrapped == dTO.Wrapped &&
                PurchaseItems.SequenceEqual(dTO.PurchaseItems); 
        }
    }
}