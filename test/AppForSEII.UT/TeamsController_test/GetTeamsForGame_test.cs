using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs;
using AppForSEII.API.DTOs.TeamDTOs;
using AppForSEII.API.Models;
using NuGet.Protocol.Core.Types;

namespace AppForSEII.UT.TeamsController_test
{
    public class GetTeamsForGame_test: AppForSEII4SqliteUT
    {

        public GetTeamsForGame_test()
        {
            /*
            var soccer = dbContext.Sports.FirstOrDefault(s => s.Name == "Soccer");
            var basketball = dbContext.Sports.FirstOrDefault(s => s.Name == "Basketball");
            var firstUser = dbContext.Users.FirstOrDefault(u => u.Name == "Elena");
            */

            var soccer = new Sport("Soccer", 2, 3, "Soccer", "The referee has authority");
            var basketball = new Sport("Basketball", 2, 3, "Basketball","Don't touch with feet");
            var firstUser = new ApplicationUser("0", "Elena", "Navarro", new DateOnly(2026, 11, 10), 30, Gender.Female, "elena@uclm.es");

            var game = new Game(1, "Friendly match", 
                                new DateTime(2026, 11, 15), 
                                "Bernabeu", 
                                "A friendly soccer game", 
                                soccer, 
                                firstUser, 
                                new List<GameInvitation>());

            var teams = new List<Team>()
            {
                new Team(1, "Soccer Stars", "A team created in 2019", 12, 18, 40, soccer, firstUser,new List<GameInvitation>()),
                new Team(2, "Wild Animals", "A team created in 2005", 12, 18, 40, soccer, firstUser, new List<GameInvitation>()),
                new Team(3, "Tall Beasts", "A team created in 2007", 12, 18, 40, basketball, firstUser, new List<GameInvitation>())
            };

            var gameInvitations = new List<GameInvitation>()
            {
                new GameInvitation(teams[0], game, true, "The game is starting"),
                new GameInvitation(teams[1], game, true, "The game is starting")
            };

            foreach(var invitation in gameInvitations)
            {
                game.GameInvitations!.Add(invitation);
                invitation.TeamInvited!.GameInvitations!.Add(invitation);
            }

            

            _context.Add(soccer);
            _context.Add(basketball);
            _context.Add(firstUser);
            _context.Add(game);
            _context.AddRange(teams);
            _context.AddRange(gameInvitations);
            _context.SaveChanges();

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetTeamsBothValid_test()
        {
            // Arrange
            var mock = new Mock<ILogger<TeamsController>>();
            ILogger<TeamsController> logger = mock.Object;
            TeamsController controller = new TeamsController(logger, _context);

            var expectedTeams = new List<TeamDTO>()
            {
                new TeamDTO(1, "Soccer Stars", 18, 40, 12, "Elena", "Navarro", "Soccer", new List<TeamInvitationDTO>()),
                new TeamDTO(2, "Wild Animals", 18, 40, 12, "Elena", "Navarro", "Soccer", new List<TeamInvitationDTO>())
            };

            // Act
            var result = await controller.GetTeamsForGame("Soccer", "Elena");

            // Assert 
            var okResult = Assert.IsType<OkObjectResult>(result);
            var teamsActualResult = Assert.IsType<List<TeamDTO>>(okResult.Value);
            Assert.Equal(expectedTeams, teamsActualResult);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetTeamsNULLSportDefaultSport()
        {
            // Arrange 
            var mock = new Mock<ILogger<TeamsController>>();
            ILogger<TeamsController> logger = mock.Object;
            TeamsController controller = new TeamsController(logger, _context);

            var expectedTeams = new List<TeamDTO>()
            {
                new TeamDTO(1, "Soccer Stars", 18, 40, 12, "Elena", "Navarro", "Soccer", new List<TeamInvitationDTO>()),
                new TeamDTO(2, "Wild Animals", 18, 40, 12, "Elena", "Navarro", "Soccer", new List<TeamInvitationDTO>())
            };

            // Act
            var result = await controller.GetTeamsForGame(null, "Elena");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var teamsActualResult = Assert.IsType<List<TeamDTO>>(okResult.Value);
            Assert.Equal(expectedTeams, teamsActualResult);
        }
        
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetTeamsUnknownUserReturnsEmpty()
        {
            // Arrange 
            var mock = new Mock<ILogger<TeamsController>>();
            ILogger<TeamsController> logger = mock.Object;
            TeamsController controller = new TeamsController(logger, _context);

            // Act
            var result = await controller.GetTeamsForGame("Soccer", "Carlos");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var teams = Assert.IsType<List<TeamDTO>>(okResult.Value);
            Assert.Equal(new List<TeamDTO>(), teams);
        }

    }    
}

