using Colabora.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Colabora.Infrastructure.Persistence.Configuration.EntityTypeConfigurations;

public class VolunteerEntityTypeConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("VOLUNTEER");

        builder.HasKey(volunteer => volunteer.Id);
        builder.Property(volunteer => volunteer.Id).IsRequired().ValueGeneratedOnAdd();
        builder.HasAlternateKey(volunteer => volunteer.Email);
    }
}