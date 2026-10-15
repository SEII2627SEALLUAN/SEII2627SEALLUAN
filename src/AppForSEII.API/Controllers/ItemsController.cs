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
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<Item>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetItemsForPurchase()
        {
            IList<Item> items = await _context.Items.ToListAsync();
            return Ok(items);
        }
    }
}