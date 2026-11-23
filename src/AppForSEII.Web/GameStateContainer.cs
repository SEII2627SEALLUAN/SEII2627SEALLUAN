using AppForSEII.Web.API;

namespace AppForSEII.Web
{
    public class GameStateContainer
    {
        public GameForCreateDTO Game { get; private set; } = new GameForCreateDTO()
        {
            Invitations = new List<GameInvitationDTO>()
        };

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AddTeamToGame(TeamForGameDTO team)
        {
            if (!Game.Invitations.Any(gi => gi.Name == team.Name))
            {
                Game.Invitations.Add(new GameInvitationDTO()
                {
                    Name = team.Name,
                    MinAge = team.MinAge,
                    MaxAge = team.MaxAge,
                    MaxMembers = team.MaxMembers,
                    CaptainName = team.CaptainName,
                    CaptainSurname = team.CaptainSurname,
                    SportName = team.SportName,
                    Message = "Dear Team, you are invited to play"
                });
            }
        }

        public void RemoveTeamFromGame(GameInvitationDTO team)
        {
            Game.Invitations.Remove(team);
        }

        public void ClearTeamsInvited()
        {
            Game.Invitations.Clear();
        }

        public void GameProcessed()
        {
            Game = new GameForCreateDTO()
            {
                Invitations = new List<GameInvitationDTO>()
            };
            NotifyStateChanged();
        }

    }
}