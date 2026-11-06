using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs;
using AppForSEII.API.DTOs.TeamDTOs;
using AppForSEII.API.Models;
using Castle.Core.Logging;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using NuGet.Protocol.Core.Types;

namespace AppForSEII.UT.GamesController_test
{
    public class CreateGame_test: AppForSEII4SqliteUT
    {
        private const string _captain = "elena@uclm.es";
        private const string _message = "The game is starting";
        private const string _place = "Bernabeu";
        private const string _description = "A new game";

        public CreateGame_test()
        {
            var soccer = new Sport("Soccer", 2, 3, "Soccer", "The referee has authority");
            var basketball = new Sport("Basketball", 2, 3, "Basketball","Don't touch with feet");
            var firstUser = new ApplicationUser("0", "Elena", "Navarro", new DateOnly(2026, 11, 10), 30, Gender.Female, "elena@uclm.es");
            var notCaptainUser = new ApplicationUser("1", "Carlos", "Gómez", new DateOnly(2000, 11, 10), 27, Gender.Male, "carlos@uclm.es");

            var game = new Game(1, "Friendly match", 
                                new DateTime(2026, 11, 15), 
                                "Bernabeu", 
                                "A friendly soccer game", 
                                soccer, 
                                firstUser, 
                                new List<GameInvitation>());

            var teams = new List<Team>()
            {
                new Team(1, "Soccer Stars", "A team created in 2019", 12, 18, 40, soccer, firstUser, new List<GameInvitation>()),
                new Team(2, "Wild Animals", "A team created in 2005", 12, 18, 40, soccer, firstUser,new List<GameInvitation>()),
                new Team(3, "Tall Beasts", "A team created in 2007", 12, 18, 40, basketball, firstUser,new List<GameInvitation>()),
                new Team(4, "Galactic Beasts", "A team created in 2007", 12, 18, 40, soccer, notCaptainUser, new List<GameInvitation>())
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
            _context.Add(notCaptainUser);
            _context.Add(game);
            _context.AddRange(teams);
            _context.AddRange(gameInvitations);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreateGame()
        {
            var futureDate = DateTime.Today.AddDays(3);

            var validInvitations = new List<GameTeamForCreateDTO>()
            {
                new GameTeamForCreateDTO("Soccer Stars", _message),
                new GameTeamForCreateDTO("Wild Animals", _message)
            };

            var notUniqueName = new GameForCreateDTO("Friendly match", futureDate, _place, _message, _description, validInvitations, _captain);

            var dateInThePast = new GameForCreateDTO("New match", DateTime.Today.AddDays(-1), _place, _message, _description, validInvitations, _captain);

            var notEnoughTeams = new GameForCreateDTO("New match", futureDate, _place, _message, _description, 
                                                        new List<GameTeamForCreateDTO>() {new GameTeamForCreateDTO("Soccer Stars", _message)}, _captain);

            var unknownCaptain = new GameForCreateDTO("New match", futureDate, _place, _message, _description, validInvitations, "john@uclm.es");

            var unknownTeam = new GameForCreateDTO("New match", futureDate, _place, _message, _description, 
                                                    new List<GameTeamForCreateDTO>()
                                                    {
                                                        new GameTeamForCreateDTO("Soccer Stars", _message),
                                                        new GameTeamForCreateDTO("Wild Animals", _message),
                                                        new GameTeamForCreateDTO("Fake team", _message)
                                                    },
                                                    _captain
                                                    );   

            var noCaptain = new GameForCreateDTO("New match", futureDate, _place, _message, _description, validInvitations, "carlos@uclm.es");  

            return new List<object[]>
            {
                new object[] { notUniqueName, "A game name must be unique" },
                new object[] { dateInThePast, "The game date cannot be in the past."},
                new object[] { notEnoughTeams, "There should be at least"},
                new object[] { unknownCaptain, "Error! UserName is not registered"}, 
                new object[] { unknownTeam, "Team 'Fake team'not available " },
                new object[] { noCaptain, "Error! The user is not the captain of any of the invited teams"}
            };


        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateGame))]
        public async Task CreateGameError_test(GameForCreateDTO gameDTO, string errorExpected)
        {
            // Arrange
            var mock = new Mock<ILogger<GamesController>>();
            ILogger<GamesController> logger = mock.Object;
            GamesController controller = new GamesController(_context, logger);

            // Act
            var result = await controller.CreateGame(gameDTO);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequest.Value);
            var errorActual = problemDetails.Errors.First().Value[0];
            Assert.StartsWith(errorExpected, errorActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GameErrorWhileSaving_test()
        {
            // Arrange
            var mock = new Mock<ILogger<GamesController>>();
            ILogger<GamesController> logger = mock.Object;
            GamesController controller = new GamesController(_context, logger);

            // WE use NULL in the place attribute to force an error
            var gameDTO = new GameForCreateDTO("New match", DateTime.Today.AddDays(3), null, _message, _description,
                                new List<GameTeamForCreateDTO>()
                                {
                                    new GameTeamForCreateDTO("Soccer Stars", _message),
                                    new GameTeamForCreateDTO("Wild Animals", _message)
                                },
                                _captain);

            // Act
            var result = await controller.CreateGame(gameDTO);

            // Assert 
            var conflict = Assert.IsType<ConflictObjectResult>(result);
            Assert.StartsWith("Error", (string)conflict.Value!);

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateGameSuccess_test()
        {
            // Arrange
            var mock = new Mock<ILogger<GamesController>>();
            ILogger<GamesController> logger = mock.Object;
            GamesController controller = new GamesController(_context, logger);

            var gameForCreate = new GameForCreateDTO("New match", DateTime.Today.AddDays(30), _place, _message, _description, 
                                                    new List<GameTeamForCreateDTO>()
                                                    {
                                                        new GameTeamForCreateDTO("Soccer Stars", _message),
                                                        new GameTeamForCreateDTO("Wild Animals", _message)
                                                    },
                                                    _captain
                                                    );

            var expectedGame = new GameDTO(2, "New match", DateTime.Today.AddDays(30), _place, _description, "Soccer", 
                                            new List<GameInvitationDTO>()
                                            {
                                                new GameInvitationDTO(1, "Soccer Stars", 18, 40, 12, "Elena", "Navarro", "Soccer", _message),
                                                new GameInvitationDTO(2, "Wild Animals", 18, 40, 12, "Elena", "Navarro", "Soccer", _message)
                                            });

            // Act
            var result = await controller.CreateGame(gameForCreate);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetGame", createdResult.ActionName);
            Assert.Equal(2, createdResult.RouteValues!["id"]);

            var actualGame = Assert.IsType<GameDTO>(createdResult.Value);
            Assert.Equal(expectedGame, actualGame);

            // We create a new context because otherwise 
            // the objects are still on memory and the test always succeds
            using var newContext = CreateContext();   
            var savedGame = newContext.Games
                .Include(g => g.Sport)
                .Include(g => g.GameInvitations)
                .FirstOrDefault(g => g.Name == "New match");

            Assert.NotNull(savedGame);
            Assert.Equal("Soccer", savedGame.Sport.Name);
            Assert.Equal(2, savedGame.GameInvitations.Count);
            Assert.Equal(2, newContext.Games.Count());
        }

    }
}