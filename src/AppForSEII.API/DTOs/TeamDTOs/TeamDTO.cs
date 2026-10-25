namespace AppForSEII.API.Models;


public class TeamDTO
{
    public TeamDTO()
    {
        
    }

    public TeamDTO(int id, string name, int minAge, int maxAge, int maxMembers, string captainName, string captainSurname, string sportName)
    {
        Id = id;
        Name = name;
        MinAge = minAge;
        MaxAge = maxAge;
        MaxMembers = maxMembers;
        CaptainName = captainName;
        SportName = sportName;
        CaptainSurname = captainSurname;
    }

    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = "Team name";
    [Range(1, 100)]
    public int MinAge { get; set; } = 3;
    [Range(1, 100)]
    public int MaxAge { get; set; } = 3;
    public int MaxMembers { get; set; }

    public string? CaptainName { get; set; } 

    public string? CaptainSurname { get; set; }
    public string SportName { get; set; } = "Sport name";

    public override bool Equals(object? obj)
    {
        return obj is TeamDTO dTO &&
               Id == dTO.Id &&
               Name == dTO.Name &&
               MinAge == dTO.MinAge &&
               MaxAge == dTO.MaxAge &&
               MaxMembers == dTO.MaxMembers &&
               CaptainName == dTO.CaptainName &&
               CaptainSurname == dTO.CaptainSurname &&
               SportName == dTO.SportName;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, MinAge, MaxAge, MaxMembers, CaptainName, CaptainSurname, SportName);
    }
}