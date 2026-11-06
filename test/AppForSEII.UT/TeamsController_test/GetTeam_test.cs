using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.TeamDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Xunit;

namespace AppForSEII.UT.TeamsController_test
{
    public class GetTeam_test : AppForSEII4SqliteUT
    {
        public GetTeam_test()
        {
            // setting up test sport
            var sport = new Sport(
                name: "Soccer",
                minimumNumberOfPlayers: 2,
                numberofRefeeres: 1,
                description: "Description of Soccer",
                basicRules: "Rules of Soccer"
            );

            // setting up test captain
            var captain = new ApplicationUser(
                "1",
                "Elena",
                "Navarro Martínez",
                new DateOnly(1985, 5, 20),
                41,
                Gender.Female,
                "elena@uclm.es"
            );

            // setting up test team and assigning the captain
            var team = new Team(
                id: 1,
                name: "Soccer Stars",
                description: "A team created in 1999",
                maxMembers: 12,
                minAge: 18,
                maxAge: 40,
                sport: sport,
                applicationUser: captain,
                gameInvitations: new List<GameInvitation>()
            );
            // team.ApplicationUser = captain;

            _context.Sports.Add(sport);
            _context.ApplicationUsers.Add(captain);
            _context.Teams.Add(team);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetTeam_successful_test()
        {
            // arrange
            Mock<ILogger<TeamsController>> mockLogger = new Mock<ILogger<TeamsController>>();
            TeamsController sut = new TeamsController(mockLogger.Object, _context);

            List<TeamInvitationDTO> expectedInvitations = new List<TeamInvitationDTO>();
            
            TeamDTO expectedTeam = new TeamDTO(
                1,
                "Soccer Stars",
                18,
                40,
                12,
                "Elena",
                "Navarro Martínez",
                "Soccer",
                expectedInvitations
            );

            // act
            var result = await sut.GetTeam(1);

            // assert
            var okresult = Assert.IsType<OkObjectResult>(result);
            var teamDetailActual = Assert.IsType<TeamDTO>(okresult.Value);
            Assert.Equal(expectedTeam, teamDetailActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetTeam_notfound_test()
        {
            // arrange
            Mock<ILogger<TeamsController>> mockLogger = new Mock<ILogger<TeamsController>>();
            TeamsController sut = new TeamsController(mockLogger.Object, _context);

            // act 
            var result = await sut.GetTeam(0);

            // assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}