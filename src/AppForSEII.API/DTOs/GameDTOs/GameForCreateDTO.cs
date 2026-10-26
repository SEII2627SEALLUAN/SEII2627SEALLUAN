using System.ComponentModel.DataAnnotations;
namespace AppForSEII.API.Models;

public class GameForCreateDTO
{
    public GameForCreateDTO() {}

    [Required]
    public string? Name { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 6)]
    public string? Place { get; set; }

    [Required]
    public string? Message { get; set; }

    public string? Description { get; set; }

    public List<GameTeamForCreateDTO> Invitations { get; set; }

    public string CreatorUserName { get; set; }

}

public class GameTeamForCreateDTO
{
    [Required]
    public string? TeamName { get; set; }

    public string? RequestMessage { get; set; }
}

