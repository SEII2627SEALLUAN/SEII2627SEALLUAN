namespace AppForSEII.API.Models;

public class Team
{
    public Team()
    {
        
    }
    public Team(int id, string name, string description, int maxMembers, int minAge, int maxAge, Sport? sport, ApplicationUser applicationUser, IList<GameInvitation>? gameInvitations)
    {
        Id = id;
        Name = name;
        Description = description;
        MaxMembers = maxMembers;
        MinAge = minAge;
        MaxAge = maxAge;
        Sport = sport;
        GameInvitations = gameInvitations;
        ApplicationUser = applicationUser;
    }

    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = "Team name";

    public string Description { get; set; } = "Team name";

    public int MaxMembers { get; set; }

    [Range(3, 100)]
    public int MinAge { get; set; } = 3;

    [Range(3, 100)]
    public int MaxAge { get; set; } = 3;

    public int SportId { get; set; }      
    public Sport? Sport { get; set; }
    public string? ApplicationUserId { get; set; }

    [ForeignKey(nameof(ApplicationUserId))]
    public ApplicationUser? ApplicationUser { get; set; }

    public IList<GameInvitation>? GameInvitations { get; set; }
    public IList<TeamInvitation>? TeamInvitations { get; set; }

    

}