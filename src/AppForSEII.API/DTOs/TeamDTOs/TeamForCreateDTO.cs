using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AppForSEII.API.DTOs.TeamDTOs
{
    public class TeamForCreateDTO
    {
        public TeamForCreateDTO()
        {
            
        }

        public TeamForCreateDTO(string name, string? description, int maxMembers, int minAge, int maxAge,
            string sportName, string captainUserName, IList<TeamInvitationDTO> teamInvitations)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            MaxMembers = maxMembers;
            MinAge = minAge;
            MaxAge = maxAge;
            SportName = sportName ?? throw new ArgumentNullException(nameof(sportName));
            CaptainUserName = captainUserName ?? throw new ArgumentNullException(nameof(captainUserName));
            TeamInvitations = teamInvitations ?? throw new ArgumentNullException(nameof(teamInvitations));
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set the name of the team")]
        [StringLength(9, MinimumLength = 4, ErrorMessage = "Team name must be between 4 and 9 characters")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Range(1, 100, ErrorMessage = "Please, set the maximum number of players")]
        public int MaxMembers { get; set; }

        [Range(3, 100, ErrorMessage = "Minimum age must be at least 3")]
        public int MinAge { get; set; } = 3;

        [Range(3, 100, ErrorMessage = "Please, set the maximum age of the team")]
        public int MaxAge { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, select the sport of the team")]
        public string SportName { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set the user name of the captain")]
        public string CaptainUserName { get; set; } = string.Empty;

        public IList<TeamInvitationDTO> TeamInvitations { get; set; } = new List<TeamInvitationDTO>();

        public override bool Equals(object? obj)
        {
            return obj is TeamForCreateDTO other &&
                   Name == other.Name &&
                   Description == other.Description &&
                   MaxMembers == other.MaxMembers &&
                   MinAge == other.MinAge &&
                   MaxAge == other.MaxAge &&
                   SportName == other.SportName &&
                   CaptainUserName == other.CaptainUserName &&
                   TeamInvitations.SequenceEqual(other.TeamInvitations);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Name);
            hash.Add(Description);
            hash.Add(MaxMembers);
            hash.Add(MinAge);
            hash.Add(MaxAge);
            hash.Add(SportName);
            hash.Add(CaptainUserName);
            foreach (var invitation in TeamInvitations) hash.Add(invitation);
            return hash.ToHashCode();
        }
    }
}