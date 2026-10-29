using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace AppForSEII.API.DTOs.TeamDTOs
{
    public class TeamDTO
    {
        public TeamDTO()
        {
            TeamInvitations = new List<TeamInvitationDTO>();
        }

        public TeamDTO(int id, string name, int minAge, int maxAge, int maxMembers, string captainName, string captainSurname, int captainAge, string sportName, IList<TeamInvitationDTO> teamInvitations)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            MinAge = minAge;
            MaxAge = maxAge;
            MaxMembers = maxMembers;
            CaptainName = captainName ?? throw new ArgumentNullException(nameof(captainName));
            CaptainSurname = captainSurname ?? throw new ArgumentNullException(nameof(captainSurname));
            CaptainAge = captainAge;
            SportName = sportName ?? throw new ArgumentNullException(nameof(sportName));
            TeamInvitations = teamInvitations ?? throw new ArgumentNullException(nameof(teamInvitations));
        }

        public TeamDTO(int id, string name, int minAge, int maxAge, int maxMembers, string captainName, string sportName)
            : this(id, name, minAge, maxAge, maxMembers, captainName, "Unknown", 0, sportName, new List<TeamInvitationDTO>())
        {
        }

        public int Id { get; set; }

        [Required]
        [StringLength(9, MinimumLength = 4, ErrorMessage = "Team name must be between 4 and 9 characters")]
        public string Name { get; set; } = "Team name";

        [Range(1, 100)]
        public int MinAge { get; set; } = 3;

        [Range(1, 100)]
        public int MaxAge { get; set; } = 18;

        public int MaxMembers { get; set; }

        [Required]
        public string CaptainName { get; set; } = "Captain name";

        [Required]
        public string CaptainSurname { get; set; } = "Captain surname";

        [Range(1, 100)]
        public int CaptainAge { get; set; } = 18;

        [Required]
        public string SportName { get; set; } = "Sport name";

        public IList<TeamInvitationDTO> TeamInvitations { get; set; } = new List<TeamInvitationDTO>();

        public override bool Equals(object? obj)
        {
            return obj is TeamDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   MinAge == dTO.MinAge &&
                   MaxAge == dTO.MaxAge &&
                   MaxMembers == dTO.MaxMembers &&
                   CaptainName == dTO.CaptainName &&
                   CaptainSurname == dTO.CaptainSurname &&
                   CaptainAge == dTO.CaptainAge &&
                   SportName == dTO.SportName &&
                   TeamInvitations.SequenceEqual(dTO.TeamInvitations);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, MinAge, MaxAge, CaptainName, CaptainSurname, SportName);
        }
    }
}