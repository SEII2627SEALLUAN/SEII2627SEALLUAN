using System.ComponentModel.DataAnnotations;
namespace AppForSEII.API.Models;
public class GameDTO
{
    public GameDTO()
    {
        
    }

    public GameDTO(int id, string? name, DateTime date, string? place, string? description, string sportName, IList<GameInvitationDTO> gameInvitations)
    {
        Id = id;
        Name = name;
        Date = date;
        Place = place;
        Description = description;
        SportName = sportName;
        GameInvitations = gameInvitations;
    }

    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }
    
    [Required]
    public DateTime? Date { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string? Place { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string SportName { get; set; } = "Sport name";

    public IList<GameInvitationDTO> GameInvitations { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is GameDTO dTO &&
               Id == dTO.Id &&
               Name == dTO.Name &&
               Date == dTO.Date &&
               Place == dTO.Place &&
               Description == dTO.Description &&
               SportName == dTO.SportName &&
               EqualityComparer<IList<GameInvitationDTO>>.Default.Equals(GameInvitations, dTO.GameInvitations);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Date, Place, Description, SportName, GameInvitations);
    }
}