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

    public int TeamId { get; set; }

    [ForeignKey(nameof(TeamId))]
    public Team TeamInvited { get; set; } = null!;

    public int GameId { get; set; }

    [ForeignKey(nameof(GameId))]
    public Game GameInvited { get; set; } = null!;

    public bool AcceptedGame { get; set; } = false;
    public string Message { get; set; } = "Message more than 10 chars";
}