
namespace AppForSEII.API.Models;

public class Referee : ApplicationUser
{

    public int Rating{get; set;}
    public int YearsRefereeing{get; set;}

    public IList<RefereeAssignedTo>? RefereeAssignedTos {get; set;}

    public Sport SportForReferee {get; set;} = new Sport();
   
}