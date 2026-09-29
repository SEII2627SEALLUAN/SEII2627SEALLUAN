namespace AppForSEII.API.Models;
public class Sport
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = "Game";
    [Required]
    public int MinimumNumberOfPlayers { get; set; } = 2;
    [Required]
    public int NumberofRefeeres { get; set; } = 1;
    [Required]
    public string Description { get; set; } = "Description";
    [Required]
    public string BasicRules { get; set; } = "Rules to the game";

    public Game? Game { get; set; }

    public IList<Game> Games { get; set; } = new List<Game>();

    public IList<Team> Teams { get; set; } = new List<Team>();

}