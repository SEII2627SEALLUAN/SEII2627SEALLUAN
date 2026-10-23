using AppForSEII.API.Data;
using AppForSEII.API.DTOs.TeamDTOs;
using System.Net;

namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TeamsController> _logger;

        public TeamsController(ILogger<TeamsController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]

        public async Task<ActionResult> ComputeDivision(decimal op1, decimal op2)

        {

            if (op2 == 0)

            {

                _logger.LogError($"{DateTime.Now} Exception: op2=0, division by 0");

                return BadRequest("op2 must be different from 0");

            }

            decimal result = decimal.Round(op1 / op2, 2);

            return Ok(result);

        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<TeamDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetTeamsForGame(string? sport, string? captainName)
        {
            if (string.IsNullOrEmpty(sport))
            {
                sport = await _context.Sports
                    .OrderBy(s => s.Id)
                    .Select(s => s.Name)
                    .FirstOrDefaultAsync();
            }


            IList<TeamDTO> teams = await _context.Teams
            .Include(m => m.Sport)
            .Include(m => m.ApplicationUser)
            .Where(m => (sport == null || (m.Sport != null && m.Sport.Name == sport))
                 && (captainName == null || (m.ApplicationUser != null && m.ApplicationUser.Name == captainName)))
            .Select(m => new TeamDTO(
                        m.Id,
                        m.Name,
                        m.MinAge,
                        m.MaxAge,
                        m.MaxMembers,
                        m.ApplicationUser.Name,
                        m.ApplicationUser.Surname,
                        18,
                        m.Sport != null ? m.Sport.Name : "No sport",
                        new List<TeamInvitationDTO>()
                    ))
                    .ToListAsync();
            return Ok(teams);
        }

        // GET: api/Teams
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TeamDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetTeam(int id)
        {
            if (_context.Teams == null)
            {
                _logger.LogError("Error: Teams table does not exist");
                return NotFound();
            }

            var teamdto = await _context.Teams
                .Where(t => t.Id == id)
                .Include(t => t.Sport)
                .Include(t => t.ApplicationUser)
                .Include(t => t.TeamInvitations!)
                    .ThenInclude(ti => ti.ApplicationUser!)
                .Select(t => new TeamDTO(
                    t.Id,
                    t.Name,
                    t.MinAge,
                    t.MaxAge,
                    t.MaxMembers,
                    t.ApplicationUser!.Name!,
                    t.ApplicationUser!.Surname!,
                    t.ApplicationUser!.Age,
                    t.Sport!.Name,
                    t.TeamInvitations!
                        .Select(ti => new TeamInvitationDTO(
                            ti.ApplicationUser!.Name!,
                            ti.ApplicationUser!.Surname!,
                            ti.ApplicationUser!.Age,
                            ti.InvitationMessage
                        )).ToList<TeamInvitationDTO>()
                ))
                .FirstOrDefaultAsync();

            if (teamdto == null)
            {
                _logger.LogError($"Error: Team with id {id} does not exist");
                return NotFound();
            }

            return Ok(teamdto);
        }

    }
}