
namespace AppForSEII.API.Models;

[Index(nameof(Name), IsUnique = true)]
[Index(nameof(GameId), IsUnique = true)]
public class RefereeGroup
{
    public RefereeGroup()
    {
    }

    public RefereeGroup( string name, string? description, string? rules, int gameId, IList<RefereeAssignedTo>? refereeAssignedTos, Game game)
    {
        Name = name;
        Description = description;
        Rules = rules;
        GameId = gameId;
        RefereeAssignedTos = refereeAssignedTos;
        Game = game;
    }

    [Key]
    public int Id{get; set;}

    [StringLength(20, MinimumLength = 3, ErrorMessage = "Length of RG name must be between 3 and 20.")]
    public string Name{get; set;} = "Name between 3 and 20 chars";
    public string? Description { get; set; }


    public string? Rules{get; set;}

    public int GameId{get; set;}

    public IList<RefereeAssignedTo>? RefereeAssignedTos {get; set;}

    public Game Game {get; set;} = new Game();


}