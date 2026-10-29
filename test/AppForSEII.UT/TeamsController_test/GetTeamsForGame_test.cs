using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs;
using AppForSEII.API.Models;
using NuGet.Protocol.Core.Types;

namespace AppForSEII.UT.TeamsController_test
{
    class GetTeamsForGame_test
    {

        public GetTeamsForGame_test(ApplicationDbContext dbContext)
        {
            /*
            var soccer = dbContext.Sports.FirstOrDefault(s => s.Name == "Soccer");
            var basketball = dbContext.Sports.FirstOrDefault(s => s.Name == "Basketball");
            var firstUser = dbContext.Users.FirstOrDefault(u => u.Name == "Elena");
            */

            var soccer = new Sport("Soccer", 2, 3, "Soccer", "The referee has authority");
            var firstUser = new ApplicationUser("0", "Elena", "Navarro", new DateOnly(2026, 11, 10), 30, Gender.Female, "elena@uclm.es");

            var gameInvitations = new List<GameInvitation>()
            {
                new GameInvitation()
            };
            
            /*
            foreach (var data in teamsData)
            {
                var team = new Team(
                    id: 0,
                    name: data.name,
                    description: data.Description,
                    maxMembers: data.MaxMembers,
                    minAge: data.MinAge,
                    maxAge: data.MaxAge,
                    sport: data.Sport,
                    gameInvitations: null
                );
                dbContext.Teams.Add(team);
                var captain = dbContext.Users.Include(u => u.Team).FirstOrDefault(u => u.UserName == data.Captain);

                if (captain != null)
                {
                    captain.Team.Add(team);
                }

                dbContext.SaveChanges();

                var gameInvitations = new List<GameInvitation>()
                {
                    new GameInvitation(0, );
                };

                var games = new List<Game>()
                {
                    // new Game(0, "The classic game", new DateTime(2026, 11, 19), "Sea Stadium", "A game near the sea.", soccer, firstUser, )
                };

            }
            */
            

        }

        
        public async Task GetTeamsForGameDoesntExist_test(TeamsController controller)
        {
            
        }
            

    }    
}

