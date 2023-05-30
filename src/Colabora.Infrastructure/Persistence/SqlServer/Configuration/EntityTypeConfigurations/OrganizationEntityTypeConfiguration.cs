using Colabora.Domain.Organization;
using Colabora.Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Colabora.Infrastructure.Persistence.SqlServer.Configuration.EntityTypeConfigurations;

public class OrganizationEntityTypeConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("ORGANIZATION");

        builder.HasKey(organization => organization.OrganizationId);
        builder.Property(organization => organization.OrganizationId).IsRequired().ValueGeneratedOnAdd();

        builder.HasAlternateKey(organization => organization.Email);
        builder.Property(organization => organization.Email).IsRequired();

        builder.Property(organization => organization.Name).IsRequired();
        
        builder.Property(organization => organization.State)
            .IsRequired()
            .HasConversion(  
                s => s.ToString(), 
                s => (States)Enum.Parse(typeof(States), s));

        builder.Property(organization => organization.Interests)
            .IsRequired()
            .HasConversion(
                interests => string.Join(",", interests.Select(i => i.ToString())),
                interests => interests.Split(",", StringSplitOptions.None).Select(i => (Interests) Enum.Parse(typeof(Interests), i)).ToList());

        builder.Property(organization => organization.CreatedBy).IsRequired();
        
        builder.Property(organization => organization.CreatedAt).HasDefaultValueSql("NOW()");
    }
}