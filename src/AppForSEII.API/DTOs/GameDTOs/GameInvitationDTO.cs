using System.ComponentModel.DataAnnotations;
namespace AppForSEII.API.Models;

public class GameInvitationDTO: TeamDTO
{
    public GameInvitationDTO()
    {
        
    }

    public GameInvitationDTO(int id, string name, int minAge, int maxAge, int maxMembers, 
                            string captainName, string captainSurname, string sportName, string message) 
                            : base(id, name, minAge, maxAge, maxMembers, captainName, captainSurname, sportName)
    {
        Message = message;
    }

    public string Message { get; set; } = "Default message";


}