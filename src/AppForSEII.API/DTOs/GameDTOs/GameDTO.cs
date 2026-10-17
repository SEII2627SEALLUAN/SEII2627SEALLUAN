using System.ComponentModel.DataAnnotations;
namespace AppForSEII.API.Models;
public class GameDTO
{
    public GameDTO()
    {
        
    }

    public GameDTO(int id, string? name, DateTime date, string? place, string? description, string sportName, IList<TeamDTO> teams)
    {
        Id = id;
        Name = name;
        Date = date;
        Place = place;
        Description = description;
        SportName = sportName;
        Teams = teams;
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

    public IList<TeamDTO> Teams { get; set; }

}