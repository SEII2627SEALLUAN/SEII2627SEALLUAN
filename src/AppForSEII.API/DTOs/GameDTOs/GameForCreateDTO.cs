using System.ComponentModel.DataAnnotations;
namespace AppForSEII.API.Models;

public class GameForCreateDTO
{
    public GameForCreateDTO() {}

    public GameForCreateDTO(string? name, DateTime date, string? place, string? description, IList<GameInvitationDTO> invitations, string creatorUserName)
    {
        Name = name;
        Date = date;
        Place = place;
        Description = description;
        Invitations = invitations;
        CreatorUserName = creatorUserName;
    }

    [Required]
    public string? Name { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 6)]
    public string? Place { get; set; }

    public string? Description { get; set; }

    public IList<GameInvitationDTO> Invitations { get; set; }

    public string CreatorUserName { get; set; } = String.Empty;

    public override bool Equals(object? obj)
    {
        return obj is GameForCreateDTO dTO &&
               Name == dTO.Name &&
               Date == dTO.Date &&
               Place == dTO.Place &&
               Description == dTO.Description &&
               Invitations.SequenceEqual(dTO.Invitations) &&
               CreatorUserName == dTO.CreatorUserName;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Date, Place, Description, CreatorUserName);
    }
}