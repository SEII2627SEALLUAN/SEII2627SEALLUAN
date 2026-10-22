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
            .Include(m=> m.Sport)
            .Include(m=> m.ApplicationUser)
            .Where(m => (sport == null || (m.Sport != null && m.Sport.Name == sport))
                 && (captainName == null || (m.ApplicationUser != null && m.ApplicationUser.Name == captainName)))
            .Select(m=>new TeamDTO(m.Id, 
                                    m.Name, 
                                    m.MinAge, 
                                    m.MaxAge, 
                                    m.MaxMembers, 
                                    m.ApplicationUser != null ? m.ApplicationUser.Name : "No captain",
                                    m.ApplicationUser != null ? m.ApplicationUser.Surname : "",
                                    m.Sport != null ? m.Sport.Name : "No sport"))
            .ToListAsync();
            return Ok(teams);
        }

    }
}