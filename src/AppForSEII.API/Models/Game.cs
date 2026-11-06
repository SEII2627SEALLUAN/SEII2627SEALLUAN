namespace AppForSEII.API.Models;
public class Game
{

    public Game()
    {
        
    }

    public Game(int id, string? name, DateTime date, string? place, string? description, Sport? sport, ApplicationUser? responsibleFor, IList<GameInvitation>? gameInvitations)
    {
        Id = id;
        Name = name;
        Date = date;
        Place = place;
        Description = description;
        Sport = sport;
        ResponsibleFor = responsibleFor;
        GameInvitations = gameInvitations;
    }

    [Key]
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }

    [Required]
    public DateTime Date{get;set;}

    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string? Place { get; set; } = "More than 5 chars";

    public string? Description { get; set; }

    public Sport? Sport { get; set; }

    public ApplicationUser? ResponsibleFor { get; set; }

    public IList<GameInvitation>? GameInvitations { get; set; } = new List<GameInvitation>();

    public override bool Equals(object? obj)
    {
        return obj is Game game &&
               Id == game.Id &&
               Name == game.Name &&
               Date == game.Date &&
               Place == game.Place &&
               Description == game.Description &&
               EqualityComparer<Sport?>.Default.Equals(Sport, game.Sport) &&
               EqualityComparer<ApplicationUser?>.Default.Equals(ResponsibleFor, game.ResponsibleFor) &&
               EqualityComparer<IList<GameInvitation>?>.Default.Equals(GameInvitations, game.GameInvitations);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Date, Place, Description, Sport, ResponsibleFor, GameInvitations);
    }
}