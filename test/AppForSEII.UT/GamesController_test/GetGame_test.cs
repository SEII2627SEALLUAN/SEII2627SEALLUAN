using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs;
using AppForSEII.API.DTOs.TeamDTOs;
using AppForSEII.API.Models;
using NuGet.Protocol.Core.Types;

namespace AppForSEII.UT.GamesController_test
{
    public class GetGame_test: AppForSEII4SqliteUT
    {
        public GetGame_test() {
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
                new Team(1, "Soccer Stars", "A team created in 2019", 12, 18, 40, soccer, new List<GameInvitation>()),
                new Team(2, "Wild Animals", "A team created in 2005", 12, 18, 40, soccer, new List<GameInvitation>()),
                new Team(3, "Tall Beasts", "A team created in 2007", 12, 18, 40, basketball, new List<GameInvitation>())
            };

            teams[0].ApplicationUser = firstUser;
            teams[1].ApplicationUser = firstUser;
            teams[2].ApplicationUser = firstUser;

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
        public async Task GetCorrectGame_test()
        {
            // Arrange
            var mock = new Mock<ILogger<GamesController>>();
            ILogger<GamesController> logger = mock.Object;
            GamesController controller = new GamesController(_context, logger);


            var expectedGame = new GameDTO(1, 
                                            "Friendly match", 
                                            new DateTime(2026, 11, 15), 
                                            "Bernabeu", 
                                            "A friendly soccer game", 
                                            "Soccer", 
                                            new List<GameInvitationDTO>()
                                            {
                                                new GameInvitationDTO(1, "Soccer Stars", 18, 40, 12, "Elena", "Navarro", 30, "Soccer", "The game is starting"),
                                                new GameInvitationDTO(2, "Wild Animals", 18, 40, 12, "Elena", "Navarro", 30, "Soccer", "The game is starting")
                                            });

            // Act
            var result = await controller.GetGame(1);

            // Assert 
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualGame = Assert.IsType<GameDTO>(okResult.Value);
            Assert.Equal(expectedGame, actualGame);


        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GameDoesntExist_test()
        {
            // Arrange
            var mock = new Mock<ILogger<GamesController>>();
            ILogger<GamesController> logger = mock.Object;
            GamesController controller = new GamesController(_context, logger);

            // Act
            var result = await controller.GetGame(22);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Game with id 22 not found.", notFoundResult.Value);
        }

    }
}