using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models;

[PrimaryKey(nameof(UserId), nameof(TeamId))]
public class TeamInvitation
{
    public TeamInvitation()
    {
    }

    public TeamInvitation(ApplicationUser applicationUser, Team team, bool invitationAccepted, string invitationMessage)
    {
        ApplicationUser = applicationUser ?? throw new ArgumentNullException(nameof(applicationUser));
        UserId = applicationUser.Id;
        Team = team ?? throw new ArgumentNullException(nameof(team));
        TeamId = team.Id;
        InvitationAccepted = invitationAccepted;
        InvitationMessage = invitationMessage ?? string.Empty;
    }

    public string UserId { get; set; } = "0";

    [ForeignKey(nameof(UserId))]
    public ApplicationUser ApplicationUser { get; set; } = null!;

    public int TeamId { get; set; }

    [ForeignKey(nameof(TeamId))]
    public Team Team { get; set; } = null!;

    public bool InvitationAccepted { get; set; } = false;

    public string InvitationMessage { get; set; } = "Invitation Message for a team member";
}