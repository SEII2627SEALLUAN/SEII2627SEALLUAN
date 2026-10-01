using AppForSEII.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; 
namespace AppForSEII.API.Data {
    public class SeedData {
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

            foreach (string roleName in roles) {
                //it checks such role does not exist in the database 
                if (!roleManager.RoleExistsAsync(roleName).Result) {
                    IdentityRole role = new IdentityRole();
                    role.Name = roleName;
                    role.NormalizedName = roleName;
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }

        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager, List<string> roles) {
            //first, it checks the user does not already exist in the DB
            if (userManager.FindByNameAsync("elena@uclm.es").Result == null) {
                ApplicationUser user = new ApplicationUser("1", "Elena", "Navarro Martínez", "elena@uclm.es");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "Password1234%");
                result.Wait();

                if (result.IsCompletedSuccessfully) {
                    //administrator role
                    userManager.AddToRoleAsync(user, roles[0]).Wait();
                }
            }


            if (userManager.FindByNameAsync("peter@uclm.es").Result == null) {
                //A customer class has been defined because it has different attributes (purchase, rental, etc.)
                ApplicationUser user = new ApplicationUser("3", "Peter", "Jackson", "peter@uclm.es");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "OtherPass12$");

                result.Wait();

                if (result.IsCompletedSuccessfully) {
                    //customer role
                    userManager.AddToRoleAsync(user, roles[2]).Wait();

                }
            }



        }

        public static void SeedSportsAndItems(ApplicationDbContext dbcontext) {
            //Sport
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
            dbcontext.SaveChanges(); //Added Sport WITH NEW ID            
            //Item
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
                item.Sport = sports[0];            // attach the Soccer sport
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
                item.Sport = sports[1];            // attach the Basketball sport
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
                item.Sport = sports[2];            // attach the Baseball sport
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
                item.Sport = sports[3];            // attach the Tennis sport
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
                if (dbcontext.Teams.FirstOrDefault (t => t.Name == data.name) != null)
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



    }
}