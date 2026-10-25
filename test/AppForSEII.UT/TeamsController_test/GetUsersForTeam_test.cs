using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.ApplicationUserDTO;
using AppForSEII.API.DTOs.TeamDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppForSEII.UT.TeamsController_test
{
    public class GetUsersForTeam_test : AppForSEII4SqliteUT
    {
        public GetUsersForTeam_test()
        {
            var sport = new Sport(
                name: "Soccer",
                minimumNumberOfPlayers: 2,
                numberofRefeeres: 1,
                description: "Description of Soccer",
                basicRules: "Rules of Soccer"
            );

            var captain = new ApplicationUser(
                "1",
                "Elena",
                "Navarro Martínez",
                new DateOnly(1985, 5, 20),
                41,
                Gender.Female,
                "elena@uclm.es"
            );

            var team = new Team(
                id: 1,
                name: "Soccer Stars",
                description: "A team created in 1999",
                maxMembers: 12,
                minAge: 18,
                maxAge: 40,
                sport: sport,
                gameInvitations: null
            );
            team.ApplicationUser = captain;

            _context.Sports.Add(sport);
            _context.ApplicationUsers.Add(captain);
            _context.Teams.Add(team);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetTeam_Successful_Test()
        {
            // Arrange
            Mock<ILogger<TeamsController>> mockLogger = new Mock<ILogger<TeamsController>>();
            TeamsController sut = new TeamsController(mockLogger.Object, _context);


            // Act
            List<TeamInvitationDTO> expectedInvitations = new List<TeamInvitationDTO>();
            

            TeamDTO expectedTeam = new TeamDTO(
                1,
                "Soccer Stars",
                18,
                40,
                12,
                "Elena",
                "Navarro Martínez",
                41,
                "Soccer",
                expectedInvitations
            );

            var result = await sut.GetTeam(1);

            // Assert
            var okresult = Assert.IsType<OkObjectResult>(result);
            var teamDetailActual = Assert.IsType<TeamDTO>(okresult.Value);
            Assert.Equal(expectedTeam, teamDetailActual);
        }

        [Fact]
        public async Task GetTeam_NotFound_test()
        {
            // Arrange
            Mock<ILogger<TeamsController>> mockLogger = new Mock<ILogger<TeamsController>>();
            TeamsController sut = new TeamsController(mockLogger.Object, _context);

            // Act
            var result = await sut.GetTeam(0);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}