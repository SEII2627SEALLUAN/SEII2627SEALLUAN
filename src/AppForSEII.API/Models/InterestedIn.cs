namespace AppForSEII.API.Models;

public class InterestedIn
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey(nameof(Sport))]
    public int SportId { get; set; } 
    [Required]
    public string UserId { get; set; } = "0";

    public int Skill { get; set; } = 5;
    public Sport Sport {get; set;} = null!;
    public Team Team { get; set; } = null!;
    public ApplicationUser ApplicationUser {get; set;} = null!;
}