using System.ComponentModel.DataAnnotations;
using AppForSEII.API.DTOs.TeamDTOs;
namespace AppForSEII.API.Models;

public class GameInvitationDTO: TeamDTO
{
    public GameInvitationDTO()
    {
        
    }

    public GameInvitationDTO(int id, string name, int minAge, int maxAge, int maxMembers, 
                            string captainName, string captainSurname, string sportName, string message) 
                            : base(id, name, minAge, maxAge, maxMembers, captainName, captainSurname, 18, sportName, new List<TeamInvitationDTO>())
    {
        Message = message;
    }

    public string Message { get; set; } = "Default message";


}