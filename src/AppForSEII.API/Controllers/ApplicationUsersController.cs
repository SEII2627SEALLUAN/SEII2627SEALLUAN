using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.ApplicationUserDTO;
using AppForSEII.API.Models; 
using AppForSEII.API.DTOs.TeamDTOs;



namespace AppForSEII.API.Controllers
{
    public class ApplicationUsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;        
        private readonly ILogger<ApplicationUsersController> _logger;

        public ApplicationUsersController(ApplicationDbContext context, ILogger<ApplicationUsersController> logger)
        {
            _context = context;
            _logger = logger;
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
        [ProducesResponseType(typeof(IList<ApplicationUserDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Index()
        {
            var entities = await _context.ApplicationUsers
                .Include(u => u.InterestedIn)
                    .ThenInclude(i => i.Sport)
                .ToListAsync();

            IList<ApplicationUserDTO> users = entities
                .Select(u => new ApplicationUserDTO(
                    u.Id, u.Name, u.Surname, u.Age, u.UserName, u.PhoneNumber,
                    u.InterestedIn.Count > 0
                        ? string.Join(", ", u.InterestedIn.OrderBy(i => i.Sport.Name).Select(i => i.Sport.Name + ": " + i.Skill))
                        : "No sports"
                ))
                .ToList();

            return Ok(users);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ApplicationUserDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsersForTeam(string? sport, int? age, int? skill = null)
        {
            if (string.IsNullOrEmpty(sport))
            {
                var defaultSport = await _context.Sports.FirstOrDefaultAsync();
                if (defaultSport != null)
                {
                    sport = defaultSport.Name;
                }
            }

            var entities = await _context.ApplicationUsers
                .Include(u => u.InterestedIn)
                    .ThenInclude(i => i.Sport)
                .Where(u => 
                    (age == null || u.Age == age) &&
                    ((sport == null && skill == null) ||
                        (u.InterestedIn != null && u.InterestedIn.Any(i => i.Sport != null
                            && (sport == null || i.Sport.Name == sport)
                            && (skill == null || i.Skill >= skill))))
                )
                .OrderBy(u => u.Name)
                    .ThenBy(u => u.Surname)
                .ToListAsync();

            IList<ApplicationUserDTO> users = entities
                .Select(u => new ApplicationUserDTO(u.Id, u.Name, u.Surname, u.Age, u.UserName, u.PhoneNumber,
                    u.InterestedIn.Count > 0
                        ? string.Join(", ", u.InterestedIn
                            .OrderBy(i => i.Sport.Name)
                            .Select(i => i.Sport.Name + ": " + i.Skill))
                        : "No sports"
                ))
                .ToList();

            return Ok(users);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TeamDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTeam(int id)
        {
            var team = await _context.Teams
                .Include(t => t.ApplicationUser) // Captain
                .Include(t => t.Sport)
                .Include(t => t.TeamInvitations!)
                    .ThenInclude(ti => ti.ApplicationUser!) // Invited players
                .FirstOrDefaultAsync(t => t.Id == id);

            if (team == null)
            {
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