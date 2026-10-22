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
                g.GameInvitations.Select(gi => new GameInvitationDTO(
                    gi.TeamInvited.Id,
                    gi.TeamInvited.Name,
                    gi.TeamInvited.MinAge,
                    gi.TeamInvited.MaxAge,
                    gi.TeamInvited.MaxMembers,
                    gi.TeamInvited.ApplicationUser.Name,
                    gi.TeamInvited.ApplicationUser.Surname,
                    gi.TeamInvited.Sport != null ? gi.TeamInvited.Sport.Name : "No sport",
                    gi.Message
                )).ToList<GameInvitationDTO>()
            )).FirstOrDefaultAsync();

            if (Game == null)
            {
                return NotFound($"Game with id {id} not found.");
            }
            return Ok(Game);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(GameDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateGame(GameForCreateDTO gameForCreate)
        {
            if (await _context.Games.AnyAsync(g => g.Name == gameForCreate.Name))
            {
                ModelState.AddModelError("Name", "A game name must be unique");
            }
            
            if (gameForCreate.Date <= DateTime.Today)
            {
                ModelState.AddModelError("Date", "The game date cannot be in the past.");
            }

            if (gameForCreate.Place.Length <= 5) {
                ModelState.AddModelError("Place", "The place must be more than 5 characters long.");
            }

            if (gameForCreate.Message.Length <= 10)
            {
                ModelState.AddModelError("Message", "The message must be more than 10 characters long.");
            }

            if (gameForCreate.Invitations.Count == 0)
            {
                ModelState.AddModelError("Invitations", "At least one invitation must be provided.");
            }

            // TODO: Check that the user exists 

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            return Ok();

        }

    }

}