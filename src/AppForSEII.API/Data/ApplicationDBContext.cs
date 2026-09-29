using AppForSEII.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII.API.DTOs.ApplicationUserDTO;

namespace AppForSEII.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {

        base.OnModelCreating(builder);


    }

<<<<<<< HEAD
    public DbSet<Game> Game { get; set; }
    public DbSet<Sport> Sport { get; set; }
=======
>>>>>>> 6fab59cf825487e998c1eea99fa28ebfde7a3561

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    public DbSet<RefereeGroup> RefereeGroups {get; set;}
    
    public DbSet<Item> Items{get;set;}

    public DbSet<Purchase> Purchases{get;set;}
}