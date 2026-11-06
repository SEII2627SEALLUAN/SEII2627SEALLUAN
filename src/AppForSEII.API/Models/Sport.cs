namespace AppForSEII.API.Models;
[Index(nameof(Name), IsUnique = true)]
public class Sport
{
    public Sport()
    {
    }
    public Sport(int id, string name, int minimumNumberOfPlayers, int numberofRefeeres, string description, string basicRules)
    {
        Id = id;
        Name = name;
        MinimumNumberOfPlayers = minimumNumberOfPlayers;
        NumberofRefeeres = numberofRefeeres;
        Description = description;
        BasicRules = basicRules;
    }

    public Sport( string name, int minimumNumberOfPlayers, int numberofRefeeres, string description, string basicRules)
    {
      
        Name = name;
        MinimumNumberOfPlayers = minimumNumberOfPlayers;
        NumberofRefeeres = numberofRefeeres;
        Description = description;
        BasicRules = basicRules;
    }

    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = "Game";
    [Range(2, 100)]
    public int MinimumNumberOfPlayers { get; set; } = 2;
    [Range(1, 10)]
    public int NumberofRefeeres { get; set; } = 1;
    [Required]
    public string Description { get; set; } = "Description";
    [Required]
    public string BasicRules { get; set; } = "Rules to the game";
    public List<Item> Items { get; set; }
    public IList<InterestedIn> InterestedIn { get; set; } = new List<InterestedIn>();

    // 1:N collection property for Teams
    public IList<Team> Team { get; set; } = new List<Team>();

    public override bool Equals(object? obj)
    {
        return obj is Sport sport &&
               Id == sport.Id &&
               Name == sport.Name &&
               MinimumNumberOfPlayers == sport.MinimumNumberOfPlayers &&
               NumberofRefeeres == sport.NumberofRefeeres &&
               Description == sport.Description &&
               BasicRules == sport.BasicRules &&
               EqualityComparer<List<Item>>.Default.Equals(Items, sport.Items) &&
               EqualityComparer<IList<InterestedIn>>.Default.Equals(InterestedIn, sport.InterestedIn) &&
               EqualityComparer<IList<Team>>.Default.Equals(Team, sport.Team);
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(Id);
        hash.Add(Name);
        hash.Add(MinimumNumberOfPlayers);
        hash.Add(NumberofRefeeres);
        hash.Add(Description);
        hash.Add(BasicRules);
        hash.Add(Items);
        hash.Add(InterestedIn);
        hash.Add(Team);
        return hash.ToHashCode();
    }
}