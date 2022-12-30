using Colabora.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Colabora.Infrastructure.Persistence.Configuration.EntityTypeConfigurations;

public class SocialActionEntityTypeConfiguration : IEntityTypeConfiguration<SocialAction>
{
    public void Configure(EntityTypeBuilder<SocialAction> builder)
    {
        builder.ToTable("SOCIAL_ACTION");

        builder.HasKey(action => action.Id);
        builder.Property(action => action.Id).IsRequired().ValueGeneratedOnAdd();
    }
}