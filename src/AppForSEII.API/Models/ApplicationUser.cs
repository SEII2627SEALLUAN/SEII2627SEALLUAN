using Microsoft.AspNetCore.Identity;

namespace AppForSEII.API.Models;

public class ApplicationUser : IdentityUser
{
public ApplicationUser()
{
}

    public ApplicationUser(string id, string name, string surname, DateOnly birthDate, int age, Gender gender, string email)
    {
        Id = id;
        Name = name;
        Surname = surname;
        BirthDate = birthDate;
        Age = age;
        Gender = gender;
        Email = email;
        UserName = email;
    }


public string? Name { get; set; }
public string? Surname { get; set; }
public DateOnly BirthDate { get; set; }
public int Age { get; set; }
public Gender Gender { get; set; }
public IList<InterestedIn> InterestedIn {get; set;} = new List<InterestedIn>();
public IList<TeamInvitation> TeamInvitations {get; set;}= new List<TeamInvitation>();
public IList<Team> Team {get; set;} = new List<Team>();

    public override bool Equals(object? obj)
    {
        return obj is ApplicationUser user &&
               Name == user.Name &&
               Surname == user.Surname &&
               BirthDate.Equals(user.BirthDate) &&
               Age == user.Age &&
               Gender == user.Gender &&
               EqualityComparer<IList<InterestedIn>>.Default.Equals(InterestedIn, user.InterestedIn) &&
               EqualityComparer<IList<TeamInvitation>>.Default.Equals(TeamInvitations, user.TeamInvitations) &&
               EqualityComparer<IList<Team>>.Default.Equals(Team, user.Team);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Surname, BirthDate, Age, Gender, InterestedIn, TeamInvitations, Team);
    }
}
