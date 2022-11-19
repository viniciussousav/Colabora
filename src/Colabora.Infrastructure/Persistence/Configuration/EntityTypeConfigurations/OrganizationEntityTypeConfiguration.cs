using Colabora.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Colabora.Infrastructure.Persistence.Configuration.EntityTypeConfigurations;

public class OrganizationEntityTypeConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("Organizations");

        builder.HasKey(organization => organization.Id);
        builder.Property(organization => organization.Id).IsRequired().ValueGeneratedOnAdd();
        builder.HasOne<Volunteer>().WithMany().HasForeignKey(organization => organization.CreatedBy);
    }
}