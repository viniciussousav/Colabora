using Colabora.Domain.Entities;
using Colabora.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Colabora.Infrastructure.Persistence.Configuration.EntityTypeConfigurations;

public class ParticipationEntityTypeConfiguration : IEntityTypeConfiguration<Participation>
{
    public void Configure(EntityTypeBuilder<Participation> builder)
    {
        builder.HasKey(participation => new {participation.SocialActionId, participation.VolunteerId});
        
        builder
            .HasOne<Volunteer>()
            .WithMany()
            .HasForeignKey(participation => participation.VolunteerId)
            .HasConstraintName("FK_VOLUNTEER");
        
        builder
            .HasOne<SocialAction>()
            .WithMany()
            .HasForeignKey(participation => participation.SocialActionId)
            .HasConstraintName("FK_SOCIAL_ACTION");
        
        builder.Property(participation => participation.JoinedAt).IsRequired();
    }
}