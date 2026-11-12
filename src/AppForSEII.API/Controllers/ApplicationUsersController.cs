using AppForSEII.API.DTOs.ApplicationUserDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.ApplicationUserDTO;
using AppForSEII.API.Models; 
using AppForSEII.API.DTOs.TeamDTOs;



namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [ProducesResponseType(typeof(IList<ApplicationUserDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Index()
        {
            IList<ApplicationUserDTO> userDTOs = await _context.ApplicationUsers
                .Include(u => u.InterestedIn)
                    .ThenInclude(i => i.Sport)
                .OrderBy(u => u.Name)
                    .ThenBy(u => u.Surname)
                .Select(u => new ApplicationUserDTO(
                    u.Id, 
                    u.Name, 
                    u.Surname, 
                    u.Age, 
                    u.UserName, 
                    u.PhoneNumber,
                    u.InterestedIn.Count > 0
                        ? string.Join(", ", u.InterestedIn
                            .OrderBy(i => i.Sport.Name)
                            .Select(i => i.Sport.Name + ": " + i.Skill))
                        : "No sports"
                ))
                .ToListAsync();

            return Ok(userDTOs);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ApplicationUserDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsersForTeam(string? sport, int? age)
        {
            
            IList<ApplicationUserDTO> userDTOs = await _context.ApplicationUsers
                .Include(u => u.InterestedIn)
                    .ThenInclude(i => i.Sport)
                .Where(u => 
                    (age == null || u.Age == age) && 
                    (sport == null || (u.InterestedIn != null && u.InterestedIn.Any(i => i.Sport != null && i.Sport.Name == sport)))
                )
                .OrderBy(u => u.Name)
                    .ThenBy(u => u.Surname)
                .Select(u => new ApplicationUserDTO(
                    u.Id, 
                    u.Name, 
                    u.Surname, 
                    u.Age, 
                    u.UserName, 
                    u.PhoneNumber
                ))
                .ToListAsync();

            return Ok(userDTOs);
        }
    }
}