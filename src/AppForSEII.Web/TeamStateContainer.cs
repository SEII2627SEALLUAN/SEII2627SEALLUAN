//This is like the RentalStateContainer
using AppForSEII.Web.API;

namespace AppForSEII.Web
{
    public class TeamStateContainer
    {
        // we create an instance of Team when an instance of TeamStateContainer is created
        public TeamForCreateDTO Team { get; private set; } = new TeamForCreateDTO()
        {
            TeamInvitations = new List<TeamInvitationDTO>()
        };

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AddUserToTeam(ApplicationUserDTO user)
        {
            // before adding a user we check whether they have been already added
            if (!Team.TeamInvitations.Any(ti => ti.UserName == user.UserName))
            {
                // we add them if they are not in the list
                Team.TeamInvitations.Add(new TeamInvitationDTO()
                {
                    UserName = user.UserName,
                    UserSurname = user.Surname,
                    UserAge = user.Age,
                    InvitationMessage = "Dear Player, join my team!"
                });
                NotifyStateChanged();
            }
        }

        // to delete users from the list of selected users
        public void RemoveUserFromTeam(TeamInvitationDTO item)
        {
            Team.TeamInvitations.Remove(item);
            NotifyStateChanged();
        }

        // we eliminate all the users from the list
        public void ClearTeamCart()
        {
            Team.TeamInvitations.Clear();
            NotifyStateChanged();
        }

        // we have already finished the process of creating a team, thus, we create a new Team
        public void TeamProcessed()
        {
            // we have finished the team creation process so we create a new object without data
            Team = new TeamForCreateDTO()
            {
                TeamInvitations = new List<TeamInvitationDTO>()
            };
            NotifyStateChanged();
        }
    }
}