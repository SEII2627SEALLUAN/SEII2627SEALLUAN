namespace AppForSEII.API.Models;

public class TeamInvitation
{
    public string UserId { get; set; } = "0";

    public int TeamId { get; set; }

    public bool InvitationAccepted { get; set; } = false;

    public string InvitationMessage { get; set; } = "Invitation Message for a team member";

    public Team Team { get; set; } = null!;
    public ApplicationUser ApplicationUser { get; set; } = null!;
}