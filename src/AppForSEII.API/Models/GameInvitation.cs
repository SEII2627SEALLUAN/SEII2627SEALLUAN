namespace AppForSEII.API.Models;

[PrimaryKey(nameof(TeamId), nameof(GameId))]
public class GameInvitation
{
    public GameInvitation()
    {
        
    }
    public GameInvitation(int teamId, Team team, int gameId, Game game, bool acceptedGame, string message)
    {
        TeamId = teamId;
        TeamInvited = team;
        GameId = gameId;
        GameInvited = game;
        AcceptedGame = acceptedGame;
        Message = message;
    }

    public GameInvitation(Team team, Game game, bool acceptedGame, string message)
    {
        TeamId = team.Id;
        TeamInvited = team;
        GameId = game.Id;
        GameInvited = game;
        AcceptedGame = acceptedGame;
        Message = message;
    }

    public int TeamId { get; set; }

    [ForeignKey(nameof(TeamId))]
    public Team TeamInvited { get; set; } = null!;

    public int GameId { get; set; }

    [ForeignKey(nameof(GameId))]
    public Game GameInvited { get; set; } = null!;

    public bool AcceptedGame { get; set; } = false;
    [StringLength(500, MinimumLength = 11, ErrorMessage = "Message more than 10 characters")]
    public string Message { get; set; } = "Message more than 10 chars";

    public override bool Equals(object? obj)
    {
        return obj is GameInvitation invitation &&
               TeamId == invitation.TeamId &&
               EqualityComparer<Team>.Default.Equals(TeamInvited, invitation.TeamInvited) &&
               GameId == invitation.GameId &&
               EqualityComparer<Game>.Default.Equals(GameInvited, invitation.GameInvited) &&
               AcceptedGame == invitation.AcceptedGame &&
               Message == invitation.Message;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(TeamId, TeamInvited, GameId, GameInvited, AcceptedGame, Message);
    }
}