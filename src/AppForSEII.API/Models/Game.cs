namespace AppForSEII.API.Models;
public class Game
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public DateTime Date{get;set;}

    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string Place { get; set; } 

    public string? Description { get; set; }

    // Add a foreign key property for the Sport entity when it's ready
    // [ForeignKey(nameof(Sport))]
    public int SportId { get; set; }

    public List<Sport> Sports { get; set; } = new List<Sport>();

}