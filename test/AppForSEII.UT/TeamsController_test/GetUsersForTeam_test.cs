using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.ApplicationUserDTO;
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
    public class GetUsersForTeam_test : AppForSEII4SqliteUT
    {
        public GetUsersForTeam_test()
        {
            // setting up test sports
            var soccer = new Sport(
                name: "Soccer",
                minimumNumberOfPlayers: 2,
                numberofRefeeres: 1,
                description: "Description of Soccer",
                basicRules: "Rules of Soccer"
            );

            var basketball = new Sport(
                name: "Basketball",
                minimumNumberOfPlayers: 5,
                numberofRefeeres: 2,
                description: "Description of Basketball",
                basicRules: "Rules of Basketball"
            );

            // setting up test users
            var user1 = new ApplicationUser("1", "Ana", "García", new DateOnly(2004, 1, 1), 22, Gender.Female, "ana@uclm.es");
            user1.PhoneNumber = "123456789";

            var user2 = new ApplicationUser("2", "Carlos", "Pérez", new DateOnly(2001, 5, 10), 25, Gender.Male, "carlos@uclm.es");
            user2.PhoneNumber = "987654321";

            var user3 = new ApplicationUser("3", "Bea", "López", new DateOnly(2004, 3, 15), 22, Gender.Female, "bea@uclm.es");
            user3.PhoneNumber = "456123789";

            // assigning sports and skills to users
            user1.InterestedIn = new List<InterestedIn> { new InterestedIn(soccer, user1.Id, 4) };
            user2.InterestedIn = new List<InterestedIn> { new InterestedIn(soccer, user2.Id, 3), new InterestedIn(basketball, user2.Id, 5) };
            user3.InterestedIn = new List<InterestedIn> { new InterestedIn(basketball, user3.Id, 2) };

            _context.Sports.Add(soccer);
            _context.Sports.Add(basketball);
            _context.ApplicationUsers.Add(user1);
            _context.ApplicationUsers.Add(user2);
            _context.ApplicationUsers.Add(user3);
            _context.SaveChanges();
        }

        public static IEnumerable<object?[]> TestCasesFor_GetUsersForTeam_OK()
        {
            var dto1 = new ApplicationUserDTO("1", "Ana", "García", 22, "ana@uclm.es", "123456789");
            var dto2 = new ApplicationUserDTO("2", "Carlos", "Pérez", 25, "carlos@uclm.es", "987654321");
            var dto3 = new ApplicationUserDTO("3", "Bea", "López", 22, "bea@uclm.es", "456123789");

            var userDTOsTC1 = new List<ApplicationUserDTO> { dto1, dto2 }
                .OrderBy(u => u.Name).ThenBy(u => u.Surname).ToList();

            var userDTOsTC2 = new List<ApplicationUserDTO> { dto1 }
                .OrderBy(u => u.Name).ThenBy(u => u.Surname).ToList();

            var userDTOsTC3 = new List<ApplicationUserDTO> { dto2, dto3 }
                .OrderBy(u => u.Name).ThenBy(u => u.Surname).ToList();

            var allTests = new List<object?[]>
            {
                // filters to apply and expected results
                new object?[] { "Soccer", (int?)null, userDTOsTC1 },
                new object?[] { "Soccer", (int?)22, userDTOsTC2 },
                new object?[] { "Basketball", (int?)null, userDTOsTC3 },
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetUsersForTeam_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetUsersForTeam_OK_test(string sport, int? age, IList<ApplicationUserDTO> expectedUsers)
        {
            // Arrange
            var mock = new Mock<ILogger<ApplicationUsersController>>();
            ILogger<ApplicationUsersController> logger = mock.Object;
            var controller = new ApplicationUsersController(_context, logger);

            // Act
            var result = await controller.GetUsersForTeam(sport, age);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var userDTOsActual = Assert.IsType<List<ApplicationUserDTO>>(okResult.Value);
            Assert.Equal(expectedUsers, userDTOsActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetUsersForTeam_badrequest_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ApplicationUsersController>>();
            ILogger<ApplicationUsersController> logger = mock.Object;
            var controller = new ApplicationUsersController(_context, logger);

            // Act no match to any user
            var result = await controller.GetUsersForTeam("Tennis", 99);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var userDTOsActual = Assert.IsType<List<ApplicationUserDTO>>(okResult.Value);
            Assert.Empty(userDTOsActual);
        }
    }
}