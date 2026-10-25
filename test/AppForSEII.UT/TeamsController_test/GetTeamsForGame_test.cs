using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs;
using AppForSEII.API.Models;

namespace AppForSEII.UT.TeamsController_test
{
    class GetTeamsForGame_test
    {
        public GetTeamsForGame_test(ApplicationDbContext dbContext)
        {
            var soccer = dbContext.Sports.FirstOrDefault(s => s.Name == "Soccer");
            var basketball = dbContext.Sports.FirstOrDefault(s => s.Name == "Basketball");

            var teamsData = new[]
            {
                (name: "Soccer Stars", Description: "A team created in 1999", MaxMembers: 12, MinAge: 18, MaxAge: 40, Sport: soccer, Captain: "elena@uclm.es"),
                (name: "Basketball Legends", Description: "The tallest players in the world", MaxMembers: 12, MinAge: 18, MaxAge: 40, Sport: basketball, Captain: "elena@uclm.es")
            };

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
            }

        }

        public async Task GetTeamsForGameDoesntExist_test(TeamsController controller)
        {
            
        }
            

    }    
}

