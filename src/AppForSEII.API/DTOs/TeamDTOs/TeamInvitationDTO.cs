using System;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs.TeamDTOs{
    public class TeamInvitationDTO
    {
        public TeamInvitationDTO()
        {
        }

        public TeamInvitationDTO(string userName, string userSurname, int userAge, string invitationMessage)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            UserSurname = userSurname ?? throw new ArgumentNullException(nameof(userSurname));
            UserAge = userAge;
            InvitationMessage = invitationMessage ?? throw new ArgumentNullException(nameof(invitationMessage));
        }

        [Required]
        public string UserName { get; set; } = "Player name";

        [Required]
        public string UserSurname { get; set; } = "Player surname";

        [Range(1, 100)]
        public int UserAge { get; set; } = 18;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Invitation message must have at least 3 characters")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "Invitation message must have at least 3 characters")]
        public string InvitationMessage { get; set; } = "Join my team!";

        public override bool Equals(object? obj)
        {
            return obj is TeamInvitationDTO dTO &&
                   UserName == dTO.UserName &&
                   UserSurname == dTO.UserSurname &&
                   UserAge == dTO.UserAge &&
                   InvitationMessage == dTO.InvitationMessage;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UserName, UserSurname, UserAge, InvitationMessage);
        }
    }
}