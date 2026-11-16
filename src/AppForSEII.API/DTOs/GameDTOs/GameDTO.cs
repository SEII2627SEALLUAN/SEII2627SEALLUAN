using System.ComponentModel.DataAnnotations;
namespace AppForSEII.API.Models;
public class GameDTO : GameForCreateDTO
{
    public GameDTO()
    {
        
    }

    public GameDTO(int id, string? name, DateTime date, string? place, string? description, string sportName, IList<GameInvitationDTO> gameInvitations, string creatorUserName) : base (name, date, place, description, gameInvitations, creatorUserName)
    {
        Id = id;
        SportName = sportName;
    }

    public int Id { get; set; }

    /*
    [Required]
    public string? Name { get; set; }
    
    [Required]
    public DateTime? Date { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string? Place { get; set; } = string.Empty;

    public string? Description { get; set; }

    */
    public string SportName { get; set; } = "Sport name";

    public override bool Equals(object? obj)
    {
        return obj is GameDTO dTO &&
               base.Equals(obj) &&
               Id == dTO.Id &&
               SportName == dTO.SportName;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Id, SportName);
    }
}