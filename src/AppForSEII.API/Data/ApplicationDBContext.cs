using AppForSEII.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Composite Keys for N:N Join Entities
        builder.Entity<TeamInvitation>()
            .HasKey(ti => new { ti.UserId, ti.TeamId });

        builder.Entity<InterestedIn>()
            .HasKey(i => new { i.SportId, i.UserId });

        // Fix decimal precision warning
        builder.Entity<Item>()
            .Property(i => i.Price)
            .HasColumnType("decimal(18,2)");
    }


    public DbSet<Game> Games { get; set; }

    public DbSet<GameInvitation> GameInvitations { get; set; }

    public DbSet<Sport> Sports { get; set; } = null!;
    public DbSet<Team> Teams { get; set; } = null!;
    public DbSet<InterestedIn> InterestedIn { get; set; } = null!;
    public DbSet<TeamInvitation> TeamInvitations { get; set; } = null!;
    public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
    public DbSet<Item> Items { get; set; } = null!;
    public DbSet<Purchase> Purchases { get; set; } = null!;

    public DbSet<Referee> Referees {get; set;}
    public  DbSet<RefereeGroup> RefereeGroups  { get; set; }

}