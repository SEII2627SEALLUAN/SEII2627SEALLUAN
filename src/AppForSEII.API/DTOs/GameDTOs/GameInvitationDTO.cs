using System.ComponentModel.DataAnnotations;
using AppForSEII.API.DTOs.TeamDTOs;
namespace AppForSEII.API.Models;

public class GameInvitationDTO
{
    public GameInvitationDTO()
    {
        
    }

    public GameInvitationDTO(string name, int minAge, int maxAge, int maxMembers, string captainName, string captainSurname, string sportName, string message)
    {
        Name = name;
        MinAge = minAge;
        MaxAge = maxAge;
        MaxMembers = maxMembers;
        CaptainName = captainName;
        CaptainSurname = captainSurname;
        SportName = sportName;
        Message = message;
    }

    [Required]
    public string Name { get;set; } = "Team name";

    public int MinAge {get; set; }

    public int MaxAge { get; set; }

    public int MaxMembers { get; set; }

    public string CaptainName { get; set; } = "Captain name";

    public string CaptainSurname { get; set; } = "Captain surname";

    public string SportName { get; set; } = "Sport name";

    [StringLength(200, MinimumLength = 11, ErrorMessage = "The message must have more than 10 characters")]
    public string Message { get; set; } = "Default message";

    public override bool Equals(object? obj)
    {
        return obj is GameInvitationDTO dTO &&
               MinAge == dTO.MinAge &&
               MaxAge == dTO.MaxAge &&
               MaxMembers == dTO.MaxMembers &&
               CaptainName == dTO.CaptainName &&
               CaptainSurname == dTO.CaptainSurname &&
               SportName == dTO.SportName &&
               Message == dTO.Message;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(MinAge, MaxAge, MaxMembers, CaptainName, CaptainSurname, SportName, Message);
    }
}