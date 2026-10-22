using AppForSEII.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AppForSEII.API.Data
{
    public class SeedData
    {
        public static void Initialize(ApplicationDbContext dbContext, IServiceProvider serviceProvider, ILogger logger)
        {
            List<string> rolesNames = new List<string> { "Administrator", "Employee", "Customer" };

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            try
            {
                SeedRoles(roleManager, rolesNames);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the roles in the Database.");
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            try
            {
                SeedUsers(userManager, rolesNames);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the Users in the Database.");
            }


            try
            {
                SeedSportsAndItems(dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding Sports and Items in the Database.");
            }

            try
            {
                SeedReferees(userManager, dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding Referees in the Database.");
            }

            try
            {
                SeedTeams(dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding Teams in the Database.");
            }

            try
            {
                SeedInterestedIn(dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding InterestedIn in the Database.");
            }
        }


        public static void SeedRoles(RoleManager<IdentityRole> roleManager, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                //it checks such role does not exist in the database
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    IdentityRole role = new IdentityRole();
                    role.Name = roleName;
                    role.NormalizedName = roleName;
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager, List<string> roles)
        {
            if (userManager.FindByNameAsync("elena@uclm.es").Result == null)
            {
                ApplicationUser user = new ApplicationUser("1", "Elena", "Navarro Martínez", "elena@uclm.es");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "Password1234%");
                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
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
                    userManager.AddToRoleAsync(user, roles[2]).Wait();
                }
            }
        }


        public static void SeedReferees(UserManager<ApplicationUser> userManager, ApplicationDbContext dbcontext)
        {
            string[] sportsnames = { "Soccer", "Basketball", "Baseball" };
            List<Sport> sports = new List<Sport>();

            foreach(string sportsname in sportsnames)
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
                }

                sports.Add(sport);
            }

            if(userManager.FindByNameAsync("lucas@uclm.es").Result == null)
            {
                Referee referee = new Referee("5", "Lucas", "Lopez", "lucas@uclm.es", 10, 20, [], sports[0]);
                referee.EmailConfirmed = true;

                var result = userManager.CreateAsync(referee, "Password1234%");
                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    userManager.AddToRoleAsync(referee, "Employee").Wait();
                }
            }

            if(userManager.FindByNameAsync("john@uclm.es").Result == null)
            {
                Referee referee = new Referee("7", "John", "Smith", "john@uclm.es", 1, 0, [], sports[1]);
                referee.EmailConfirmed = true;

                var result = userManager.CreateAsync(referee, "Password1234%");
                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    userManager.AddToRoleAsync(referee, "Employee").Wait();
                }
            }

            if(userManager.FindByNameAsync("maria@uclm.es").Result == null)
            {
                Referee referee = new Referee("9", "Maria", "Martinez", "maria@uclm.es", 7, 10, [], sports[2]);
                referee.EmailConfirmed = true;

                var result = userManager.CreateAsync(referee, "Password1234%");
                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    userManager.AddToRoleAsync(referee, "Employee").Wait();
                }
            }
            
        }
        public static void SeedSportsAndItems(ApplicationDbContext dbcontext)
        {
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

            dbcontext.SaveChanges();

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
                item.Sport = sports[0];
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
                item.Sport = sports[1];
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
                item.Sport = sports[2];
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
                item.Sport = sports[3];
                dbcontext.Items.Add(item);
            }

            dbcontext.SaveChanges();
        }

        public static void SeedTeams(ApplicationDbContext dbcontext)
        {
            var soccer = dbcontext.Sports.FirstOrDefault(s => s.Name == "Soccer");
            var basketball = dbcontext.Sports.FirstOrDefault(s => s.Name == "Basketball");

            var teamsData = new[]
            {
                (name: "Soccer Stars", Description: "A team created in 1999", MaxMembers: 12, MinAge: 18, MaxAge: 40, Sport: soccer, Captain: "elena@uclm.es"),
                (name: "Basketball Legends", Description: "The tallest players in the world", MaxMembers: 12, MinAge: 18, MaxAge: 40, Sport: basketball, Captain: "elena@uclm.es")
            };

            foreach (var data in teamsData)
            {
                if (dbcontext.Teams.FirstOrDefault(t => t.Name == data.name) != null)
                {
                    continue; // Skip if the team already exists
                }

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

                dbcontext.Teams.Add(team);

                var captain = dbcontext.Users.Include(u => u.Team).FirstOrDefault(u => u.UserName == data.Captain);

                if (captain != null)
                {
                    captain.Team.Add(team);
                }
            }

            dbcontext.SaveChanges();
        }

        public static void SeedInterestedIn(ApplicationDbContext dbcontext)
        {
            var elena = dbcontext.Users.FirstOrDefault(u => u.UserName == "elena@uclm.es");
            var peter = dbcontext.Users.FirstOrDefault(u => u.UserName == "peter@uclm.es");

            var soccer = dbcontext.Sports.FirstOrDefault(s => s.Name == "Soccer");
            var tennis = dbcontext.Sports.FirstOrDefault(s => s.Name == "Tennis");
            var basketball = dbcontext.Sports.FirstOrDefault(s => s.Name == "Basketball");

            if (elena != null && soccer != null && tennis != null)
            {
                if (!dbcontext.InterestedIn.Any(i => i.ApplicationUser == elena && i.Sport == soccer))
                {
                    dbcontext.InterestedIn.Add(new InterestedIn
                    {
                        ApplicationUser = elena,
                        Sport = soccer,
                        Skill = 4
                    });
                }

                if (!dbcontext.InterestedIn.Any(i => i.ApplicationUser == elena && i.Sport == tennis))
                {
                    dbcontext.InterestedIn.Add(new InterestedIn
                    {
                        ApplicationUser = elena,
                        Sport = tennis,
                        Skill = 5
                    });
                }
            }

            if (peter != null && basketball != null)
            {
                if (!dbcontext.InterestedIn.Any(i => i.ApplicationUser == peter && i.Sport == basketball))
                {
                    dbcontext.InterestedIn.Add(new InterestedIn
                    {
                        ApplicationUser = peter,
                        Sport = basketball,
                        Skill = 3
                    });
                }
            }

            dbcontext.SaveChanges();
        }
    }
}