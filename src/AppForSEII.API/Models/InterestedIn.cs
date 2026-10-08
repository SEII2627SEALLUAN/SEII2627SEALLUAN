namespace AppForSEII.API.Models;

[PrimaryKey(nameof(UserId), nameof(SportId))]
public class InterestedIn
{
    public InterestedIn()
    {
        
    }   

    public InterestedIn(Sport sport, string userId, int skill)
    {
        Sport = sport;
        UserId = userId;
        Skill = skill;
    }
    public int SportId { get; set; }

    [ForeignKey(nameof(SportId))]
    public Sport Sport { get; set; } = null!;

    public string UserId { get; set; } = "0";

    [ForeignKey(nameof(UserId))]
    public ApplicationUser ApplicationUser { get; set; } = null!;

    [Range(1, 5)]
    public int Skill { get; set; } = 5;
}