using AppForSEII.API.DTOs.ItemDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        //enable access
        private readonly ApplicationDbContext _context;
        //log the info
        private readonly ILogger<ItemsController> _logger;
        //constructor
        public ItemsController(ApplicationDbContext context, ILogger<ItemsController> logger)
        {
            _context = context;
            _logger = logger;
        }
        //GET METHOD
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ItemDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetItemsForPurchase(string? name, Gender? gender, string? sport)
        {
                var items = await _context.Items
                    .Include(i=>i.Sport) //include sport
                    .Where(i=>((i.Name.Equals(name)) || (name==null)) //filter by name (perhaps change to contains), null includes all
                        && ((i.Sport.Name.Equals(sport)) || (sport==null)) //filter by sport, null includes all
                        && ((i.Gender==gender) || (gender==null)) //gender filter
                        && (i.QuantityAvailableForPurchase>0)) //do not show out of stock items
                        .OrderBy(i=>i.Name)
                            .ThenBy(i=>i.Price)
                            .Select(i=>new ItemDTO(i.Name, i.RecommendedAge??0, i.Gender??Gender.Other, i.Price, i.Size??"", i.Sport)) //show DTO only
                        .ToListAsync();
                    return Ok(items);

                if (items == null) //check this to see if it works as intended (QuantityAvailable check?)
                {
                    ModelState.AddModelError("noItemsAvailable", "No items available for the specified criteria");
                    _logger.LogError($"{DateTime.Now} Error: No items available for the specified criteria");
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }

        }
    }
}