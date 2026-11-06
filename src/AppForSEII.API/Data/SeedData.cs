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
                SeedGames(dbContext);
            } catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding Games in the Database.");
            }

            try
            {
                SeedInterestedIn(dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding InterestedIn in the Database.");
            }

            try
            {
                SeedTeamInvitations(dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding TeamInvitations in the Database.");
            }
        }

        public static void SeedGames(ApplicationDbContext dbcontext)
        {
            var soccer = dbcontext.Sports.FirstOrDefault(s => s.Name == "Soccer");
            var basketball = dbcontext.Sports.FirstOrDefault(s => s.Name == "Basketball");
            var elena = dbcontext.Users.FirstOrDefault(u => u.UserName == "elena@uclm.es");

            var gamesData = new[]
            {
                (Name: "Friendly match", Date: DateTime.Today.AddDays(30), Place: "Bernabeu",
                Description: "A friendly soccer game", Sport: soccer, Team: "Soccer Stars",
                Message: "The game is starting"),
                (Name: "Basketball night", Date: DateTime.Today.AddDays(45), Place: "Palau Sant Jordi",
                Description: "A basketball game", Sport: basketball, Team: "Basketball Legends",
                Message: "Join us for the game")
            };
            foreach (var data in gamesData)
            {
                if (dbcontext.Games.FirstOrDefault(g => g.Name == data.Name) != null)
                {
                    continue;
                }

                var team = dbcontext.Teams.FirstOrDefault(t => t.Name == data.Team);
                if (data.Sport == null || elena == null || team == null)
                {
                    continue;
                }

                var game = new Game(
                    id: 0,
                    name: data.Name,
                    date: data.Date,
                    place: data.Place,
                    description: data.Description,
                    sport: data.Sport,
                    responsibleFor: elena,
                    gameInvitations: new List<GameInvitation>()
                );

                game.GameInvitations.Add(new GameInvitation(team, game, true, data.Message));

                dbcontext.Games.Add(game);
            }

            dbcontext.SaveChanges();
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
            // Existing users: do not change email or id (SeedTeams / SeedTeamInvitations depend on them)
            SeedUser(userManager, "1", "Elena", "Navarro Martínez", new DateOnly(1985, 5, 20), Gender.Female, "elena@uclm.es", "Password1234%", roles[0]);
            SeedUser(userManager, "3", "Peter", "Jackson", new DateOnly(1998, 10, 12), Gender.Male, "peter@uclm.es", "OtherPass12$", roles[2]);

            // Third user: not interested in the sport (see SeedInterestedIn)
            SeedUser(userManager, "4", "María", "García López", new DateOnly(2001, 3, 15), Gender.Female, "maria@uclm.es", "Password1234%", roles[2]);
        }

        private static void SeedUser(UserManager<ApplicationUser> userManager, string id, string name, string surname,DateOnly birthDate, Gender gender, string email, string password, string role)
        {
            if (userManager.FindByNameAsync(email).Result != null) return; // already seeded

            ApplicationUser user = new ApplicationUser(id, name, surname, birthDate, CalculateAge(birthDate), gender, email);
            user.EmailConfirmed = true;

            IdentityResult result = userManager.CreateAsync(user, password).Result;
            if (!result.Succeeded)
            {   
                throw new InvalidOperationException(
            $"Could not create {email}: " + string.Join("; ", result.Errors.Select(e => e.Description)));
            }
            userManager.AddToRoleAsync(user, role).Wait();
        }

        private static int CalculateAge(DateOnly birthDate)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--; // birthday not reached yet this year
            return age;
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
                DateOnly date = new DateOnly(1985, 2, 15);
                Referee referee = new Referee(
                    "5", "Lucas", "Lopez", "lucas@uclm.es",
                    date, 41, Gender.Male,
                     10, 20, [], sports[0]);
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
                DateOnly date = new DateOnly(2000, 11, 5);
                Referee referee = new Referee(
                    "7", "John", "Smith", "john@uclm.es",
                    date, 26, Gender.Male,
                 1, 0, [], sports[1]);
                referee.EmailConfirmed = true;

                var result = userManager.CreateAsync(referee, "Password1234%");
                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    userManager.AddToRoleAsync(referee, "Employee").Wait();
                }
            }

            if(userManager.FindByNameAsync("mariaMartinez@uclm.es").Result == null)
            {
                DateOnly date = new DateOnly(1996, 3, 25);

                Referee referee = new Referee(
                    "9", "Maria", "Martinez", "mariaMartinez@uclm.es",
                    date, 30, Gender.Female,
                     7, 10, [], sports[2]);
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
                    continue;
                }

                var captain = dbcontext.Users.Include(u => u.Team).FirstOrDefault(u => u.UserName == data.Captain);

                var team = new Team(
                    id: 0,
                    name: data.name,
                    description: data.Description,
                    maxMembers: data.MaxMembers,
                    minAge: data.MinAge,
                    maxAge: data.MaxAge,
                    sport: data.Sport,
                    applicationUser: captain,
                    gameInvitations: null
                );

                dbcontext.Teams.Add(team);
            }

            dbcontext.SaveChanges();
        }

        public static void SeedInterestedIn(ApplicationDbContext dbcontext)
        {
            // Two users interested in the same sport (a sport needs at least 2 players).
            // María has no rows on purpose: she is the user not interested in the sport.
            var interests = new[]
            {
                (UserName: "elena@uclm.es", SportName: "Soccer", Skill: 4),
                (UserName: "peter@uclm.es", SportName: "Soccer", Skill: 3)
            };

            foreach (var interest in interests)
            {
                var user = dbcontext.Users.FirstOrDefault(u => u.UserName == interest.UserName);
                var sport = dbcontext.Sports.FirstOrDefault(s => s.Name == interest.SportName);
                if (user == null || sport == null) continue; // missing user or sport: skip

                if (!dbcontext.InterestedIn.Any(i => i.UserId == user.Id && i.SportId == sport.Id))
                {
                    dbcontext.InterestedIn.Add(new InterestedIn(sport, user.Id, interest.Skill));
                }
            }

            dbcontext.SaveChanges();
        }

        public static void SeedTeamInvitations(ApplicationDbContext dbcontext)
        {
            var peter = dbcontext.Users.FirstOrDefault(u => u.UserName == "peter@uclm.es");
            var team = dbcontext.Teams.FirstOrDefault(t => t.Name == "Soccer Stars");

            if (peter != null && team != null)
            {
                if (!dbcontext.TeamInvitations.Any(ti => ti.UserId == peter.Id && ti.TeamId == team.Id))
                {
                    dbcontext.TeamInvitations.Add(new TeamInvitation
                    {
                        UserId = peter.Id,
                        TeamId = team.Id,
                        InvitationAccepted = false,
                        InvitationMessage = "Join Soccer Stars team!"
                    });
                }

                dbcontext.SaveChanges();
            }
        }
    }
}