namespace AppForSEII.API.Models;

public class TeamDTO
{
    public TeamDTO()
    {
        
    }

    public TeamDTO(int id, string name, int minAge, int maxAge, int maxMembers, string captainName, string sportName)
    {
        Id = id;
        Name = name;
        MinAge = minAge;
        MaxAge = maxAge;
        MaxMembers = maxMembers;
        CaptainName = captainName;
        SportName = sportName;
    }

    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = "Team name";
    [Range(1, 100)]
    public int MinAge { get; set; } = 3;
    [Range(1, 100)]
    public int MaxAge { get; set; } = 3;
    public int MaxMembers { get; set; }

    public string CaptainName { get; set; } = "Captain name";
    public string SportName { get; set; } = "Sport name";

}