using System.ComponentModel.DataAnnotations;
namespace AppForSEII.API.Models;

public class GameForCreateDTO
{
    public GameForCreateDTO() {}

    [Required]
    public string? Name { get; set; }

    [Required]
    public DateTime? Date { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 6)]
    public string? Place { get; set; }

    [Required]
    public string? Message { get; set; }

    public string? Description { get; set; }

    [Required]
    public IList<GameInvitationForCreateDTO> Invitations { get; set; } = new List<GameInvitationForCreateDTO>();

}


public class GameInvitationForCreateDTO
{
    [Required]
    public int TeamId { get; set; }

    [Required]
    [StringLength(200, MinimumLength = 11)]
    public string Message { get; set; } = string.Empty;
}