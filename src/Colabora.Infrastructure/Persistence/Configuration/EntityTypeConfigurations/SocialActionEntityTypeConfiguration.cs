using Colabora.Domain.Entities;
using Colabora.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Colabora.Infrastructure.Persistence.Configuration.EntityTypeConfigurations;

public class SocialActionEntityTypeConfiguration : IEntityTypeConfiguration<SocialAction>
{
    public void Configure(EntityTypeBuilder<SocialAction> builder)
    {
        builder.HasKey(action => action.SocialActionId);
        builder.Property(action => action.SocialActionId).ValueGeneratedOnAdd();

        builder
            .HasOne<Organization>(action => action.Organization)
            .WithMany(organization => organization.SocialActions)
            .HasForeignKey(action => action.OrganizationId)
            .HasConstraintName("FK_ORGANIZATION");
        
        
        
        builder.Property(organization => organization.Interests)
            .IsRequired()
            .HasConversion(
                interests => string.Join(",", interests.Select(i => i.ToString())),
                interests => interests.Split(",", StringSplitOptions.None).Select(i => (Interests) Enum.Parse(typeof(Interests), i)).ToList());

    }
}