namespace AppForSEII.API.DTOs.TeamDTOs;


public class TeamForGameDTO
{
    public TeamForGameDTO() {}

    public TeamForGameDTO(int id, string name, int minAge, int maxAge, int maxMembers, string captainName, string captainSurname, string sportName)
    {
        Id = id;
        Name = name;
        MinAge = minAge;
        MaxAge = maxAge;
        MaxMembers = maxMembers;
        CaptainName = captainName;
        CaptainSurname = captainSurname;
        SportName = sportName;
    }

    public int Id { get; set; }

    [Required]
    [StringLength(9, MinimumLength = 4, ErrorMessage = "Team name must be between 4 and 9 characters")]
    public string Name { get; set; } = "Team name";

    [Range(1, 100)]
    public int MinAge { get; set; } = 3;

    [Range(1, 100)]
    public int MaxAge { get; set; } = 18;

    public int MaxMembers { get; set; }

    [Required]
    public string CaptainName { get; set; } = "Captain name";

    [Required]
    public string CaptainSurname { get; set; } = "Captain surname";

    [Required]
    public string SportName { get; set; } = "Sport name";

    public override bool Equals(object? obj)
    {
        return obj is TeamForGameDTO dTO &&
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
        return HashCode.Combine(Name, MinAge, MaxAge, MaxMembers, CaptainName, CaptainSurname, SportName);
    }
}