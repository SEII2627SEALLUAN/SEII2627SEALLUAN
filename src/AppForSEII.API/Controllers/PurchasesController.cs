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
    }
}