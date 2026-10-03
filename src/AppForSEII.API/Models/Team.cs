namespace AppForSEII.API.Models;

public class Team
{
    public Team()
    {
        
    }
    public Team(int id, string name, string description, int maxMembers, int minAge, int maxAge, Sport? sport, IList<GameInvitation>? gameInvitations)
    {
        Id = id;
        Name = name;
        Description = description;
        MaxMembers = maxMembers;
        MinAge = minAge;
        MaxAge = maxAge;
        Sport = sport;
        GameInvitations = gameInvitations;
    }

    public int Id { get; set; }

    public string Name { get; set; } = "Team name";

    public string Description { get; set; } = "Team name";

    public int MaxMembers { get; set; }

    public int MinAge { get; set; } = 3;

    public int MaxAge { get; set; } = 3;

    public Sport? Sport { get; set; }

    public IList<GameInvitation>? GameInvitations { get; set; }

}