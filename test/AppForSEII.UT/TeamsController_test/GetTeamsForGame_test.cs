using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs;
using AppForSEII.API.Models;
using NuGet.Protocol.Core.Types;

namespace AppForSEII.UT.TeamsController_test
{
    class GetTeamsForGame_test: AppForSEII4SqliteUT
    {
        private readonly ApplicationDbContext _context;

        public GetTeamsForGame_test(ApplicationDbContext dbContext)
        {
            /*
            var soccer = dbContext.Sports.FirstOrDefault(s => s.Name == "Soccer");
            var basketball = dbContext.Sports.FirstOrDefault(s => s.Name == "Basketball");
            var firstUser = dbContext.Users.FirstOrDefault(u => u.Name == "Elena");
            */

            var soccer = new Sport("Soccer", 2, 3, "Soccer", "The referee has authority");
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
                new Team(2, "Wild Animals", "A team created in 2005", 12, 18, 40, soccer, new List<GameInvitation>())
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

            

            dbContext.Add(soccer);
            dbContext.Add(firstUser);
            dbContext.Add(game);
            dbContext.AddRange(teams);
            dbContext.AddRange(gameInvitations);
            dbContext.SaveChanges();

        }

        
        public async Task GetTeamsForGameDoesntExist_test(TeamsController controller)
        {
            
        }
            

    }    
}

