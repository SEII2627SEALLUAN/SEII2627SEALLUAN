namespace AppForSEII.API.Models;

public class Team
{
    public int Id { get; set; }

    public string Name { get; set; } = "Team name";

    public string Description { get; set; } = "Team name";

    public int MaxMembers { get; set; }

    public int MinAge { get; set; } = 3;

    public int MaxAge { get; set; } = 3;
}