using AppForSEII.Web.API;

namespace AppForSEII2526.Web {
    public class PurchaseStateContainer {

        //we create an instance of Purchase when an instance of PurchaseStateContainer is created
        public PurchaseForCreateDTO Purchase { get; private set; } = new PurchaseForCreateDTO() {
            PurchaseItems = new List<PurchaseItemForCreateDTO>()
        };

        //we compute the TotalPrice of the items we have selected for purchasing them
        /*
        public decimal TotalPrice {
            get {
                int numberOfDays = (Rental.RentalDateTo - Rental.RentalDateFrom).Days;
                return Convert.ToDecimal(Rental.RentalItems.Sum(ri => ri.PriceForRenting * numberOfDays));
            }
        }
        */
        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();


        
        public void AddItemToPurchase(ItemDTO item) {
            //before adding a movie we checked whether it has been already added
            if (!Purchase.PurchaseItems.Any(pi => pi.ItemID == item.Id))
                //we add it if it is not in the list
                Rental.RentalItems.Add(new RentalItemDTO() {
                    MovieID = movie.Id,
                    Genre = movie.Genre,
                    Title = movie.Title,
                    PriceForRenting = movie.PriceForRenting,
                }
            );

        }

        //to delete movies from the list of selected movies
        public void RemoveRentalItemToRent(ItemDTO item) {
            Purchase.PurchaseItems.Remove(item);

        }

        //we eliminate all the movies from the list
        public void ClearRentingCart() {
            Purchase.PurchaseItems.Clear();

        }

        //we have already finished the process of renting, thus, we create a new Rental 
        public void PurchaseProcessed() {
            //we have finished the rental process so we create a new object without data
            Purchase = new PurchaseForCreateDTO() {
                PurchaseItems = new List<PurchaseItemForCreateDTO>()
            };
        }
    }
}