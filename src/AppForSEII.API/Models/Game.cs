namespace AppForSEII.API.Models;
public class Game
{
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

    public IList<GameInvitation>? GameInvitations { get; set; }

}