using Microsoft.AspNetCore.Identity;

namespace AppForSEII.API.Models;

public class ApplicationUser : IdentityUser
{
public ApplicationUser()
{
}

//constructor for seeding the database, attributes id and email are required by the IdentityUser class
    public ApplicationUser(string id, string name, string surname, string email)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Email = email;
        UserName = email;
    }

    public ApplicationUser(string id, string? name, string? surname, string email, DateOnly birthDate, int age, Gender gender, List<InterestedIn> interestedIn, List<TeamInvitation> teamInvitation, List<Team> team)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Email = email;
        UserName = email;
        BirthDate = birthDate;
        Age = age;
        Gender = gender;
        InterestedIn = interestedIn;
        TeamInvitation = teamInvitation;
        Team = team;
    }

    public string? Name { get; set; }
    public string? Surname { get; set; }
    public DateOnly BirthDate { get; set; }
    public int Age { get; set; }
    public Gender Gender { get; set; }
    public List<InterestedIn> InterestedIn {get; set;} = new List<InterestedIn>();
    public List<TeamInvitation> TeamInvitation {get; set;}= new List<TeamInvitation>();
    public List<Team> Team {get; set;} = new List<Team>();

}
