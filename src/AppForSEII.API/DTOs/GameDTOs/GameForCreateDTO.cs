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
    [StringLength(50, MinimumLength = 11)]
    public string? Message { get; set; }

    public string? Description { get; set; }

    public List<GameTeamForCreateDTO> Invitations { get; set; }

    public string CreatorUserName { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is GameForCreateDTO dTO &&
               Name == dTO.Name &&
               Date == dTO.Date &&
               Place == dTO.Place &&
               Message == dTO.Message &&
               Description == dTO.Description &&
               EqualityComparer<List<GameTeamForCreateDTO>>.Default.Equals(Invitations, dTO.Invitations) &&
               CreatorUserName == dTO.CreatorUserName;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Date, Place, Message, Description, Invitations, CreatorUserName);
    }
}

public class GameTeamForCreateDTO
{
    [Required]
    public string? TeamName { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 11)]
    public string? RequestMessage { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is GameTeamForCreateDTO dTO &&
               TeamName == dTO.TeamName &&
               RequestMessage == dTO.RequestMessage;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(TeamName, RequestMessage);
    }
}

