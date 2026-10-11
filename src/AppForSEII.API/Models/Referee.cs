
namespace AppForSEII.API.Models;

public class Referee : ApplicationUser
{
    public Referee()
    {
    }

    public Referee(string id, string name, string surname, string email, int rating, int yearsRefereeing, IList<RefereeAssignedTo>? refereeAssignedTos, Sport sportForReferee)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Email = email;
        Rating = rating;
        YearsRefereeing = yearsRefereeing;
        RefereeAssignedTos = refereeAssignedTos;
        SportForReferee = sportForReferee;
    }


    public int Rating{get; set;}
    public int YearsRefereeing{get; set;}

    public IList<RefereeAssignedTo>? RefereeAssignedTos {get; set;}

    public Sport SportForReferee {get; set;} = new Sport();
   
}