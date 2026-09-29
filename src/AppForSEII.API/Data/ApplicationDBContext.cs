using AppForSEII.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII.API.DTOs.ApplicationUserDTO;

namespace AppForSEII.API.Data;

public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }

    public DbSet<Sport> Sports { get; set; }
    public  DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public  DbSet<RefereeGroup> RefereeGroups  { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Purchase> Purchases { get; set; }

    public DbSet<Game> Games { get; set; }

    public DbSet<GameInvitation> GameInvitations { get; set; }

}