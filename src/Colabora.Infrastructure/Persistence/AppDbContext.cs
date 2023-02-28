using System.Diagnostics.CodeAnalysis;
using Colabora.Domain.Entities;
using Colabora.Domain.ValueObjects;
using Colabora.Infrastructure.Persistence.Configuration.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8618

namespace Colabora.Infrastructure.Persistence;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new OrganizationEntityTypeConfiguration().Configure(modelBuilder.Entity<Organization>());
        new VolunteerEntityTypeConfiguration().Configure(modelBuilder.Entity<Volunteer>());
        new SocialActionEntityTypeConfiguration().Configure(modelBuilder.Entity<SocialAction>());
        new ParticipationEntityTypeConfiguration().Configure(modelBuilder.Entity<Participation>());
    }
    
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Volunteer> Volunteers { get; set; }
    public DbSet<SocialAction> SocialActions { get; set; }
    public DbSet<Participation> Participations { get; set; }
}