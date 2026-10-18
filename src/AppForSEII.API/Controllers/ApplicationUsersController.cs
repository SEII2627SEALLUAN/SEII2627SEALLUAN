using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppForSEII.API.DTOs.ApplicationUserDTO;
using AppForSEII.API.Data;

namespace AppForSEII.API.Controllers
{
    public class ApplicationUsersController : ControllerBase
    {
        //used to eneable yur controller to acces to the database
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

        // GET: ApplicationUsers
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ApplicationUserDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.ApplicationUsers
            .Select(u=>new ApplicationUserDTO(u.Id, u.Name, u.Surname, u.Age, u.UserName, u.PhoneNumber, u.InterestedIn.FirstOrDefault() != null ? u.InterestedIn.FirstOrDefault().Skill.ToString() : "N/A"))
            .ToListAsync());
        }

                // GET: ApplicationUsers/Details/5
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ApplicationUserDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsersForTeam(string? sport, int? age, int? skill = null)//pass to paremeter the name of the sport to filter and skill is equivalence to rentalItem in diagram
        {
            //Database: join ApplicationUser -> InterestedIn -> Sport and apply the selected filters
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

            //Memory: build each DTO with the text "Sport: skill, Sport: skill"
            IList<ApplicationUserDTO> users = entities
                .Select(u => new ApplicationUserDTO(u.Id, u.Name, u.Surname, u.Age, u.UserName, u.PhoneNumber,
                    u.InterestedIn.Count > 0
                        ? string.Join(", ", u.InterestedIn
                            .OrderBy(i => i.Sport.Name)
                            .Select(i => i.Sport.Name + ": " + i.Skill))
                        : "No sports"
                        //u.InterestedIn.Select(i => new SportSkillDTO(i.Sport.Name, i.Skill)).ToList()
                ))
                .ToList();

            return Ok(users);
        }

        private bool ApplicationUserDTOExists(string name, string surname)
        {
            return _context.ApplicationUsers.Any(u => u.Name == name && u.Surname==surname);
        }
    }

    //WE have to join two tables, InterestedIn with ApplicationUser and the to Sport
    
    /*
    .Include(m=> m.InterestedIn)
    .Include(m=> m.InterestedIn.Sport)
        .ThenInclude(m=> m.InterestedIn.Sport.Skill)
        //I have to filter by the age that I pass by parameter. Using the where,select...
        { //Related with join sport and skill. Team related with sport. ApplicationUser related through InterestedIn to Sport
        */
}


/*
public async Task<IActionResult> Details(string id)//I have to filter by the age that I pass by parameter. Using the where,select...
        { //Related with join sport and skill. Team related with sport. ApplicationUser related through InterestedIn to Sport
            if (id == null)
            {
                return NotFound();
            }
            var applicationUserDTO = await _context.ApplicationUsers
                .Where(u => u.Id == id)
                .OrderBy(id => id)
                .Select(u=>new ApplicationUserDTO(u.Id, u.Name, u.Surname, u.Age, u.UserName, u.PhoneNumber, ))
                .FirstOrDefaultAsync();
            if (applicationUserDTO == null)
            {
                _logger.LogError($"Error: ApplicationUser with id {id} does not exist");

                return NotFound();
            }

            return Ok(applicationUserDTO);
        }

        public IList<RentalItemInfoDTO> RentalItemInfoDTOs { get; set; } = new List<RentalItemInfoDTO>();
        I have to pass skill and the name of the sport
*/