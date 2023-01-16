using Colabora.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Colabora.Infrastructure.Persistence.Configuration.EntityTypeConfigurations;

public class SocialActionEntityTypeConfiguration : IEntityTypeConfiguration<SocialAction>
{
    public void Configure(EntityTypeBuilder<SocialAction> builder)
    {
        builder.HasKey(action => action.SocialActionId);
        builder.Property(action => action.SocialActionId).ValueGeneratedOnAdd();
    }
}