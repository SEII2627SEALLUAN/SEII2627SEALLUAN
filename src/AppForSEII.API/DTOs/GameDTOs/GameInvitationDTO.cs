using System.ComponentModel.DataAnnotations;
namespace AppForSEII.API.Models;

public class GameInvitationDTO
{
    public GameInvitationDTO()
    {
        
    }

    public GameInvitationDTO(string message, IList<TeamDTO> teams)
    {
        Message = message;
        Teams = teams;
    }

    public string Message { get; set; } = "Default message";

    public IList<TeamDTO> Teams { get; set; }


}