using System.ComponentModel.DataAnnotations;
using AppForSEII.API.DTOs.TeamDTOs;
namespace AppForSEII.API.Models;

public class GameInvitationDTO: TeamDTO
{
    public GameInvitationDTO()
    {
        
    }

    public GameInvitationDTO(int id, string name, int minAge, int maxAge, int maxMembers, 
                            string captainName, string captainSurname, int captainAge, string sportName, string message) 
                            : base(id, name, minAge, maxAge, maxMembers, captainName, captainSurname, captainAge, sportName, new List<TeamInvitationDTO>())
    {
        Message = message;
    }

    public string Message { get; set; } = "Default message";

    public override bool Equals(object? obj)
    {
        return obj is GameInvitationDTO dTO &&
               base.Equals(obj) &&
               Message == dTO.Message;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Message);
    }
}