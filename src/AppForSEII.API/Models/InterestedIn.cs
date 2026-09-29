namespace AppForSEII.API.Models;

public class InterestedIn
{
    public int SportId { get; set; }

    public string UserId { get; set; } = "0";

    public int Skill { get; set; } = 5;
    public Team Team {get; set;}
    public ApplicationUser ApplicationUser {get; set;}
}