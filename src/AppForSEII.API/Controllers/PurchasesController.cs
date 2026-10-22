using AppForSEII.API.DTOs.ItemDTOs;
using AppForSEII.API.DTOs.PurchaseDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PurchasesController> _logger;

        public PurchasesController(ApplicationDbContext context, ILogger<PurchasesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PurchaseDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetPurchase(int id)
        {

            var purchase = await _context.Purchases
             .Where(p => p.Id == id)
                 .Include(p => p.PurchaseItems) //join table RentalItems
                    .ThenInclude(pi => pi.Item) //then join table Item
                        .ThenInclude(s => s.Sport) //then join table Sport
             .Select(p => new PurchaseDetailDTO(p.User.Name, p.User.Surname, p.PurchaseDate, p.PurchaseDescription, p.Wrapped, p.TotalPrice,
                    p.PurchaseItems //select the items in the purchase, to DTOs
                        .Select(pi => new ItemDTO(pi.Item.Name, pi.Item.RecommendedAge, pi.Item.Gender, pi.Item.Price, pi.Item.Size, pi.Item.QuantityAvailableForPurchase, pi.Item.Sport.Name)).ToList<ItemDTO>()))
             .FirstOrDefaultAsync();


            if (purchase == null)
            {
                _logger.LogError($"Error: Purchase with id {id} does not exist");
                return NotFound();
            }
            
            return Ok(purchase);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(PurchaseDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreatePurchase(PurchaseForCreateDTO purchaseForCreate)
        {
            //ERRORS
            // if (!_context.ApplicationUsers.Any(au=>au.UserName==purchaseForCreate.CustomerUserName))
            var user = _context.ApplicationUsers.FirstOrDefault(au => au.UserName == purchaseForCreate.CustomerUserName);
            if (user == null)
                ModelState.AddModelError("RentalApplicationUser", "Error! UserName is not registered");
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            //MAIN METHOD
            var itemNames = purchaseForCreate.PurchaseItems.Select(pi => pi.Name).ToList();
            var items = _context.Items
                .Where(i => itemNames.Contains(i.Name))
                .Select(i => new {
                    i.Id,
                    i.Name,
                    i.QuantityAvailableForPurchase,
                    i.Price,   
                    NumberOfPurchasedItems = i.PurchaseItems.Sum(pi => pi.Quantity) //   This checks the quantities of the PurchaseItem, to check and add later                          
                    })
                .ToList();

            Purchase purchase = new Purchase(
                DateTime.Now, 
                purchaseForCreate.PurchaseDescription, 
                0, //compute price later
                purchaseForCreate.Street,
                purchaseForCreate.City,
                purchaseForCreate.Province,
                purchaseForCreate.CreditCardNumber,
                purchaseForCreate.CCV,
                purchaseForCreate.ExpirationDate,
                purchaseForCreate.Wrapped,
                PurchaseStatus.Created,
                user.Id
            );
            //CALCULATE PRICE
            foreach (var item in purchaseForCreate.PurchaseItems) {
                var dbItem = items.FirstOrDefault(i => i.Name == item.Name);
                //check if null
                if (dbItem == null) {
                    ModelState.AddModelError("RentalItems", $"Error! The item isn't available.");
                }
                if (dbItem.NumberOfPurchasedItems + item.Quantity > dbItem.QuantityAvailableForPurchase)
                {
                    ModelState.AddModelError("RentalItems", $"Error! Only '{dbItem.QuantityAvailableForPurchase - dbItem.NumberOfPurchasedItems}' copies of '{dbItem.Name}' are available, but '{item.Quantity}' were selected.");
                }
                else {
                    // purchase does not exist in the database yet and does not have a valid Id, so we must relate
                    purchase.PurchaseItems.Add(new PurchaseItem(dbItem.Id, purchase.Id, item.Quantity, item.Price));
                    item.Price = dbItem.Price;
                }
            }
            purchase.TotalPrice = (double)purchase.PurchaseItems.Sum(pi => pi.Price * pi.Quantity); //had to cast
            
            //if there is any problem because of the available quantity of movies or because the movie does not exist
            if (ModelState.ErrorCount > 0) {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            _context.Add(purchase);

            try {
                //we store in the database both purchase and its purchaseitems
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Rental", $"Error! There was an error while saving your purchase, plese, try again later");
                return Conflict("Error" + ex.Message);

            }

            //it returns purchaseDetail
            var purchaseDetail = new PurchaseDetailDTO(purchase.User.Name,
            purchase.User.Surname,
            DateTime.Now,
            purchase.PurchaseDescription,
            purchase.Wrapped,
            purchase.TotalPrice,
            purchaseForCreate.PurchaseItems
            );

            return CreatedAtAction("GetPurchase", new { id = purchase.Id }, purchaseDetail);
        }
    }
}