using Colabora.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Colabora.Infrastructure.Persistence.Configuration.EntityTypeConfigurations;

public class MembershipEntityTypeConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.HasKey(membership => membership.MembershipId);
    }
}