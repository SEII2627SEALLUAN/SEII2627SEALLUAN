using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.TeamDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII.UT.TeamsController_test
{
    public class CreateTeam_test : AppForSEII4SqliteUT
    {
        private const string _userEmail = "elena@uclm.es";
        private const string _userName = "Elena";
        private const string _userSurname = "Navarro Martínez";
        private const string _sportName = "Soccer";
        private const string _playerEmail = "player@uclm.es";
        private const string _playerUserName = "Player";

        public CreateTeam_test()
        {
            var sport = new Sport(
                name: _sportName,
                minimumNumberOfPlayers: 2,
                numberofRefeeres: 1,
                description: "Description of Soccer",
                basicRules: "Rules of Soccer"
            );

            var captain = new ApplicationUser(
                "1",
                _userName,
                _userSurname,
                new DateOnly(1985, 5, 20),
                41,
                Gender.Female,
                _userEmail
            );

            var invitedPlayer = new ApplicationUser(
                "2",
                _playerUserName,
                "Player Surname",
                new DateOnly(1995, 1, 1),
                30,
                Gender.Male,
                _playerEmail
            );

            _context.Sports.Add(sport);
            _context.ApplicationUsers.Add(captain);
            _context.ApplicationUsers.Add(invitedPlayer);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreateTeam_Error()
        {
            var emptyInvitations = new List<TeamInvitationCreateDTO>();

            var teamSportNotExist = new TeamForCreateDTO("SocStars", "A team created", 12, 18, 40, "NonExistentSport", _userEmail, emptyInvitations);
            var teamUserNotExist = new TeamForCreateDTO("SocStars", "A team created", 12, 18, 40, _sportName, "unknown@uclm.es", emptyInvitations);

            var allTests = new List<object[]>
            {
                new object[] { teamSportNotExist, "Error! Selected sport does not exist." },
                new object[] { teamUserNotExist, "Error! Captain user is not registered" },
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateTeam_Error))]
        public async Task CreateTeam_Error_test(TeamForCreateDTO teamDTO, string errorExpected)
        {
            var mock = new Mock<ILogger<TeamsController>>();
            ILogger<TeamsController> logger = mock.Object;

            var controller = new TeamsController(logger, _context);

            var result = await controller.CreateTeam(teamDTO);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];

            Assert.StartsWith(errorExpected, errorActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateTeam_Success_test()
        {
            var mock = new Mock<ILogger<TeamsController>>();
            ILogger<TeamsController> logger = mock.Object;

            var controller = new TeamsController(logger, _context);

            var teamInvitations = new List<TeamInvitationCreateDTO>
            {
                new TeamInvitationCreateDTO(_playerEmail, "Join our team!")
            };

            var teamDTO = new TeamForCreateDTO(
                "SocStars",
                "A team created",
                12,
                18,
                40,
                _sportName,
                _userEmail,
                teamInvitations
            );

            var expectedInvitations = new List<TeamInvitationDTO>
            {
                new TeamInvitationDTO(_playerUserName, "Player Surname", 30, "Join our team!")
            };

            var expectedTeamDetail = new TeamDTO(
                1,
                "SocStars",
                18,
                40,
                12,
                _userName,
                _userSurname,
                41,
                _sportName,
                expectedInvitations
            );

            var result = await controller.CreateTeam(teamDTO);

            var okresult = Assert.IsType<CreatedAtActionResult>(result);
            var actualresult = Assert.IsType<TeamDTO>(okresult.Value);
            Assert.Equal(expectedTeamDetail, actualresult);
        }
    }
}