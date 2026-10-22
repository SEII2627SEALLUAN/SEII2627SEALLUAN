using Microsoft.EntityFrameworkCore;
namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")] 
    [ApiController] 
    public class GamesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TeamsController> _logger;

        public GamesController(ApplicationDbContext context, ILogger<TeamsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<GameDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetGame(int id)
        {
            GameDTO? Game = await _context.Games
            .Include(g => g.Sport)
            .Include(g => g.GameInvitations)
                .ThenInclude(t2 => t2.TeamInvited)
            .Where(g => g.Id == id)
            .Select(g => new GameDTO(
                g.Id, 
                g.Name, 
                g.Date, 
                g.Place, 
                g.Description, 
                g.Sport != null ? g.Sport.Name : "No sport",
                g.GameInvitations.Select(gi => new GameInvitationDTO(gi.Message, new List<TeamDTO> { new TeamDTO(gi.TeamInvited.Id, gi.TeamInvited.Name, gi.TeamInvited.MinAge, gi.TeamInvited.MaxAge, gi.TeamInvited.MaxMembers, 
                                   gi.TeamInvited.ApplicationUser != null ? 
                                   gi.TeamInvited.ApplicationUser.Name + " " + gi.TeamInvited.ApplicationUser.Surname 
                                   : "No captain", gi.TeamInvited.Sport != null ? gi.TeamInvited.Sport.Name : "No sport") })).ToList<GameInvitationDTO>()
            )).FirstOrDefaultAsync();
            return Ok(Game);
        }
    }

}