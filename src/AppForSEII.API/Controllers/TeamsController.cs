namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")] 
    [ApiController] 
    public class TeamsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TeamsController> _logger;
    }
}