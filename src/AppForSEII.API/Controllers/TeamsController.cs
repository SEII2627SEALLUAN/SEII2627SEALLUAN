using AppForSEII.API.DTOs.TeamDTOs;
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
            IList<TeamDTO> teams = await _context.Teams
            .Include(m => m.Sport)
            .Include(m => m.ApplicationUser)
            .Where(m => (sport == null || (m.Sport != null && m.Sport.Name == sport))
                 && (captainName == null || (m.ApplicationUser != null && m.ApplicationUser.Name == captainName)))
            .Select(m => new TeamDTO(m.Id, m.Name, m.MinAge, m.MaxAge, m.MaxMembers,
                                   m.ApplicationUser != null ?
                                   m.ApplicationUser.Name + " " + m.ApplicationUser.Surname
                                   : "No captain", m.Sport != null ? m.Sport.Name : "No sport"))
            .ToListAsync();
            return Ok(teams);
        }

        [HttpPost]
        [ProducesResponseType(typeof(TeamDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateTeam(TeamCreateDTO teamCreate)
        {
            // any validation defined in TeamCreateDTO is checked before running the method so they don't have to be checked again
            if (string.IsNullOrEmpty(teamCreate.Name) || teamCreate.Name.Length <= 3 || teamCreate.Name.Length >= 10)
            {
                ModelState.AddModelError("Name", "Error! Team's name must be between 4 and 9 characters.");
            }
            else if (await _context.Teams.AnyAsync(t => t.Name == teamCreate.Name))
            {
                ModelState.AddModelError("Name", "Error! Team's name must be unique.");
            }

            if (teamCreate.MaxMembers < 1)
            {
                ModelState.AddModelError("MaxMembers", "Error! You must indicate the maximum number of players.");
            }

            if (teamCreate.MinAge < 3)
            {
                ModelState.AddModelError("MinAge", "Error! Minimum age of the team must be at least 3.");
            }

            if (teamCreate.MinAge > teamCreate.MaxAge)
            {
                ModelState.AddModelError("AgeRange", "Error! Minimum age cannot be greater than maximum age.");
            }

            var captain = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == teamCreate.CaptainUserName);
            if (captain == null)
            {
                ModelState.AddModelError("Captain", "Error! Captain user is not registered.");
            }

            var sport = await _context.Sports.FirstOrDefaultAsync(s => s.Name == teamCreate.SportName);
            if (sport == null)
            {
                ModelState.AddModelError("Sport", "Error! Selected sport does not exist.");
            }

            List<ApplicationUser> invitedUsers = new List<ApplicationUser>();
            if (teamCreate.TeamInvitations == null || teamCreate.TeamInvitations.Count == 0)
            {
                ModelState.AddModelError("TeamInvitations", "Error! You must include at least one player to be invited.");
            }
            else
            {
                var userNames = teamCreate.TeamInvitations.Select(i => i.PlayerUserName).ToList();

                if (userNames.Distinct().Count() != userNames.Count)
                {
                    ModelState.AddModelError("TeamInvitations", "Error! A player cannot be invited twice.");
                }

                if (userNames.Contains(teamCreate.CaptainUserName))
                {
                    ModelState.AddModelError("TeamInvitations", "Error! The captain cannot be invited to his own team.");
                }

                invitedUsers = await _context.ApplicationUsers.Where(u => userNames.Contains(u.UserName!)).ToListAsync();

                foreach (var userName in userNames.Distinct())
                {
                    if (!invitedUsers.Any(u => u.UserName == userName))
                    {
                        ModelState.AddModelError("TeamInvitations", $"Error! Player '{userName}' is not registered.");
                    }
                }

                foreach (var invitation in teamCreate.TeamInvitations)
                {
                    if (string.IsNullOrEmpty(invitation.InvitationMessage) || invitation.InvitationMessage.Length < 3)
                    {
                        ModelState.AddModelError("InvitationMessage", "Error! Personalized invitation message must be at least 3 characters.");
                    }
                }
            }

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            Team team = new Team(
                id: 0,
                name: teamCreate.Name,
                description: teamCreate.Description ?? string.Empty,
                maxMembers: teamCreate.MaxMembers,
                minAge: teamCreate.MinAge,
                maxAge: teamCreate.MaxAge,
                sport: sport,
                gameInvitations: null
            );
            team.ApplicationUser = captain;
            team.TeamInvitations = new List<TeamInvitation>();

            List<TeamInvitationDTO> invitationDTOs = new List<TeamInvitationDTO>();

            foreach (var invitationDto in teamCreate.TeamInvitations)
            {
                var invitedUser = invitedUsers.First(u => u.UserName == invitationDto.PlayerUserName);

                // team does not exist in the database yet and does not have a valid Id, so we must relate teaminvitation to the object team
                team.TeamInvitations.Add(new TeamInvitation
                {
                    Team = team,
                    ApplicationUser = invitedUser,
                    InvitationMessage = invitationDto.InvitationMessage,
                    InvitationAccepted = false
                });

                invitationDTOs.Add(new TeamInvitationDTO(
                    invitedUser.Name ?? "Unknown",
                    invitedUser.Surname ?? "Unknown",
                    invitedUser.Age,
                    invitationDto.InvitationMessage
                ));
            }

            _context.Teams.Add(team);

            try
            {
                // we store in the database both team and its teaminvitations
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Conflict("Error! There was an error while saving your team, please, try again later");
            }

            var teamDTO = new TeamDTO(
                team.Id,
                team.Name,
                team.MinAge,
                team.MaxAge,
                team.MaxMembers,
                captain!.Name ?? "Unknown",
                captain.Surname ?? "Unknown",
                captain.Age,
                sport!.Name,
                invitationDTOs
            );

            return CreatedAtAction(nameof(GetTeam), new { id = team.Id }, teamDTO);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TeamDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTeam(int id)
        {
            var team = await _context.Teams
                .Include(t => t.ApplicationUser)
                .Include(t => t.Sport)
                .Include(t => t.TeamInvitations!)
                    .ThenInclude(ti => ti.ApplicationUser!)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (team == null)
            {
                _logger.LogError($"Error: Team with id {id} does not exist");
                return NotFound();
            }

            var captain = team.ApplicationUser;

            var invitationDTOs = team.TeamInvitations?
                .Where(ti => ti.ApplicationUser != null)
                .Select(ti => new TeamInvitationDTO(
                    ti.ApplicationUser!.Name ?? "Unknown",
                    ti.ApplicationUser!.Surname ?? "Unknown",
                    ti.ApplicationUser!.Age,
                    ti.InvitationMessage ?? string.Empty
                ))
                .ToList() ?? new List<TeamInvitationDTO>();

            var teamDTO = new TeamDTO(
                team.Id,
                team.Name ?? "Team",
                team.MinAge,
                team.MaxAge,
                team.MaxMembers,
                captain?.Name ?? "Unknown",
                captain?.Surname ?? "Unknown",
                captain?.Age ?? 0,
                team.Sport?.Name ?? "Unknown",
                invitationDTOs
            );

            return Ok(teamDTO);
        }

    }
}