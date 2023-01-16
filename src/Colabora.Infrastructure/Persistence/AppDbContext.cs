using Colabora.Domain.Entities;
using Colabora.Infrastructure.Persistence.Configuration.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Colabora.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext() { }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new OrganizationEntityTypeConfiguration().Configure(modelBuilder.Entity<Organization>());
        new VolunteerEntityTypeConfiguration().Configure(modelBuilder.Entity<Volunteer>());
    }
    
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Volunteer> Volunteers { get; set; }
    public DbSet<SocialAction> SocialActions { get; set; }
}