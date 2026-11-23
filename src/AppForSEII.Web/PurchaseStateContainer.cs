

using AppForSEII.Web.API;

namespace AppForSEII.Web {
    public class PurchaseStateContainer {

        //we create an instance of Purchase when an instance of PurchaseStateContainer is created
        public PurchaseForCreateDTO Purchase { get; private set; } = new PurchaseForCreateDTO() {
            PurchaseItems = new List<PurchaseItemDTO>()
        };

        
        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        public double TotalPrice =>
            Purchase.PurchaseItems.Sum(pi => pi.Price * pi.Quantity);
        
        public void AddItemToPurchase(PurchaseItemDTO Item) {
            //before adding a Item we checked whether it has been already added
            if (!Purchase.PurchaseItems.Any(ri => ri.Name == Item.Name)) //name is unique, acts as ID, should be fine because of DTOs
                //we add it if it is not in the list
                Purchase.PurchaseItems.Add(new PurchaseItemDTO() {
                    Name = Item.Name,
                    RecommendedAge = Item.RecommendedAge,
                    Gender = Item.Gender,
                    Price = Item.Price,
                    Size = Item.Size,
                    Quantity = Item.Quantity
                }
            );

        }

        //to delete Items from the list of selected Items
        public void RemovePurchaseItemToRent(PurchaseItemDTO item) {
            Purchase.PurchaseItems.Remove(item);

        }

        //we eliminate all the Items from the list
        public void ClearRentingCart() {
            Purchase.PurchaseItems.Clear();

        }

        //we have already finished the process of renting, thus, we create a new Purchase 
        public void PurchaseProcessed() {
            //we have finished the Purchase process so we create a new object without data
            Purchase = new PurchaseForCreateDTO() {
                PurchaseItems = new List<PurchaseItemDTO>()
            };
        }
    }
}