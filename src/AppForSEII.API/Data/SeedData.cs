using AppForSEII.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AppForSEII.API.Data
{
    public class SeedData
    {
        public static void Initialize(ApplicationDbContext dbContext, IServiceProvider serviceProvider, ILogger logger) {
            List<string> rolesNames = new List<string> { "Administrator", "Employee", "Customer" };

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            try {
                SeedRoles(roleManager, rolesNames);
            }
            catch (Exception ex) {
                logger.LogError(ex, "An error occurred seeding the roles in the Database.");
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            try {
                SeedUsers(userManager, rolesNames);
            }
            catch (Exception ex) {
                logger.LogError(ex, "An error occurred seeding the Users in the Database.");
            }

            try {
                SeedSportsAndItems(dbContext);
            }
            catch (Exception ex) {
                logger.LogError(ex, "An error occurred seeding Sports and Items in the Database.");
            }
 
            try
            {
                SeedTeams(dbContext);
            } catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding Teams in the Database.");
            }

        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager, List<string> roles) {
        {
            foreach (string roleName in roles)
            {
                // Checks if role does not exist in the database 
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    IdentityRole role = new IdentityRole();
                    role.Name = roleName;
                    role.NormalizedName = roleName;
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }
        }
    }
        public static void SeedUsers(UserManager<ApplicationUser> userManager, List<string> roles)
        {
            // First, check if the user does not already exist in the DB
            if (userManager.FindByNameAsync("elena@uclm.es").Result == null)
            {
                ApplicationUser user = new ApplicationUser("1", "Elena", "Navarro Martínez", "elena@uclm.es");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "Password1234%");
                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    // Administrator role
                    userManager.AddToRoleAsync(user, roles[0]).Wait();
                }
            }

            if (userManager.FindByNameAsync("peter@uclm.es").Result == null)
            {
                ApplicationUser user = new ApplicationUser("3", "Peter", "Jackson", "peter@uclm.es");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "OtherPass12$");
                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    // Customer role
                    userManager.AddToRoleAsync(user, roles[2]).Wait();
                }
            }
        }

        public static void SeedSportsAndItems(ApplicationDbContext dbcontext)
        {
            // 1. Seed Sports using classmate's attributes (id removed)
            string[] sportsnames = { "Soccer", "Basketball", "Baseball", "Tennis" };
            List<Sport> sports = new List<Sport>();

            foreach (string sportsname in sportsnames)
            {
                var sport = dbcontext.Sports.FirstOrDefault(s => s.Name == sportsname);
                if (sport == null)
                {
                    sport = new Sport(
                        id: 0,
                        name: sportsname,
                        minimumNumberOfPlayers: 2,
                        numberofRefeeres: 1,
                        description: "Description of " + sportsname,
                        basicRules: "Rules of " + sportsname
                    );
                    dbcontext.Sports.Add(sport);   
                }
                sports.Add(sport);                  
            }
            dbcontext.SaveChanges(); // Saves Sports and generates Identity IDs 

            // 2. Seed Items and attach corresponding Sport
            if (dbcontext.Items.FirstOrDefault(i => i.Name == "Soccer Ball") == null)
            {
                var item = new Item(
                    name: "Soccer Ball",
                    price: 20.0m,
                    recommendedAge: 10,
                    gender: null,
                    size: "M",
                    quantityAvailableForPurchase: 100
                );
                item.Sport = sports[0]; // Attach Soccer
                dbcontext.Items.Add(item);
            }

            if (dbcontext.Items.FirstOrDefault(i => i.Name == "Basketball") == null)
            {
                var item = new Item(
                    name: "Basketball",
                    price: 25.0m,
                    recommendedAge: 12,
                    gender: null,
                    size: "M",
                    quantityAvailableForPurchase: 150
                );
                item.Sport = sports[1]; // Attach Basketball
                dbcontext.Items.Add(item);
            }

            if (dbcontext.Items.FirstOrDefault(i => i.Name == "Baseball") == null)
            {
                var item = new Item(
                    name: "Baseball",
                    price: 30.0m,
                    recommendedAge: 14,
                    gender: null,
                    size: "S",
                    quantityAvailableForPurchase: 10
                );
                item.Sport = sports[2]; // Attach Baseball
                dbcontext.Items.Add(item);
            }

            if (dbcontext.Items.FirstOrDefault(i => i.Name == "Tennis Racket") == null)
            {
                var item = new Item(
                    name: "Tennis Racket",
                    price: 40.0m,
                    recommendedAge: 16,
                    gender: null,
                    size: "L",
                    quantityAvailableForPurchase: 42
                );
                item.Sport = sports[3]; // Attach Tennis
                dbcontext.Items.Add(item);
            }

            dbcontext.SaveChanges();
        }

        public static void SeedTeams(ApplicationDbContext dbcontext){
        var soccer = dbcontext.Sports.FirstOrDefault(s => s.Name == "Soccer");
        var basketball = dbcontext.Sports.FirstOrDefault(s => s.Name == "Basketball");

        var teamsData = new[]
        {
            (name: "Soccer Stars", description: "A team created in 1999", maxMembers: 12, minAge: 18, maxAge: 40, sport: soccer, captain: "elena@uclm.es"),
            (name: "Basketball Legends", description: "The tallest players in the world", maxMembers: 12, minAge: 18, maxAge: 40, sport: basketball, captain: "elena@uclm.es")
        };

        foreach (var data in teamsData)
        {
            if (dbcontext.Teams.Any(t => t.Name == data.name))
            {
                continue; // Skip if already seeded
            }

        var team = new Team(
            0,
            data.name,
            data.description,
            data.maxMembers,
            data.minAge,
            data.maxAge,
            data.sport,
            null // GameInvitations
        );

        dbcontext.Teams.Add(team);
    }

        dbcontext.SaveChanges();
    }
    }
}