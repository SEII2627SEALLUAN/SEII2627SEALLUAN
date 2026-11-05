
namespace AppForSEII.API.Models;

[PrimaryKey(nameof(RefereeGroupId), nameof(RefereeId))]
public class RefereeAssignedTo
{
    public RefereeAssignedTo()
    {
    }

    public RefereeAssignedTo(int refereeGroupId, string refereeId, bool acceptedAssignment, string role, string roleDescription, RefereeGroup refereeGroup, Referee referee)
    {
        RefereeGroupId = refereeGroupId;
        RefereeId = refereeId;
        AcceptedAssignment = acceptedAssignment;
        Role = role;
        RoleDescription = roleDescription;
        RefereeGroup = refereeGroup;
        Referee = referee;
    }

    public int RefereeGroupId {get; set;}
    public string RefereeId {get; set;} = "0";
    public bool AcceptedAssignment {get; set;} = false;

    [StringLength(20, MinimumLength = 3, ErrorMessage = "Length of RAT role must be between 3 and 20.")]
    public string Role {get; set;} = "Name of the role has to be between 3 and 20 characters";

    [StringLength(255, MinimumLength = 3, ErrorMessage = "Length of RAT role description must be between 3 and 255.")]
    public string RoleDescription {get; set;} = "Between 3 and 255 chars";

    public RefereeGroup RefereeGroup {get; set;} = new RefereeGroup();

    public Referee Referee {get; set;} = new Referee();
}