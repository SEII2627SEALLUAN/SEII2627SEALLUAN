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

    public ApplicationUser(string name, string surname, DateOnly birthDate, int age, Gender gender)
    {
        Name = name;
        Surname = surname;
        BirthDate = birthDate;
        Age = age;
        Gender = gender;
    }


    public string? Name { get; set; }
    public string? Surname { get; set; }
    public DateOnly BirthDate { get; set; }
    public int Age { get; set; }
    public Gender Gender { get; set; }
}