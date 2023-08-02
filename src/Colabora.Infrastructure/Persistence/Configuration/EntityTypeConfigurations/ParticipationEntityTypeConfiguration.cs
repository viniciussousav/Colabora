using Colabora.Domain.Participation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Colabora.Infrastructure.Persistence.Configuration.EntityTypeConfigurations;

public class ParticipationEntityTypeConfiguration : IEntityTypeConfiguration<Participation>
{
    public void Configure(EntityTypeBuilder<Participation> builder)
    {
        builder.ToTable("PARTICIPATION");
        
        builder.HasKey(participation => new {participation.SocialActionId, participation.VolunteerId});

        builder
            .HasOne(v => v.Volunteer)
            .WithMany(volunteer => volunteer.Participations)
            .HasForeignKey(participation => participation.VolunteerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(v => v.SocialAction)
            .WithMany(action => action.Participations)
            .HasForeignKey(participation => participation.SocialActionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(participation => participation.JoinedAt).IsRequired();
    }
}