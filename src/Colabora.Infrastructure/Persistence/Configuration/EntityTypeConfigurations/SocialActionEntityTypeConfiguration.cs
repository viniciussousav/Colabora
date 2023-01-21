﻿using Colabora.Domain.Entities;
using Colabora.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Colabora.Infrastructure.Persistence.Configuration.EntityTypeConfigurations;

public class SocialActionEntityTypeConfiguration : IEntityTypeConfiguration<SocialAction>
{
    public void Configure(EntityTypeBuilder<SocialAction> builder)
    {
        builder.ToTable("SOCIAL_ACTIONS");

        builder.HasKey(action => action.SocialActionId);
        builder.Property(action => action.SocialActionId).ValueGeneratedOnAdd();

        builder.Property(action => action.Title).IsRequired();
        builder.Property(action => action.Description).IsRequired();
        builder.Property(action => action.Interests).IsRequired();
        builder.Property(action => action.OccurrenceDate).IsRequired();
        builder.Property(action => action.CreatedAt).IsRequired().ValueGeneratedOnAdd();

        builder
            .HasOne<Volunteer>()
            .WithMany()
            .HasForeignKey(action => action.VolunteerCreatorId)
            .HasConstraintName("FK_VOLUNTEER");

        builder
            .HasOne<Organization>(action => action.Organization)
            .WithMany(organization => organization.SocialActions)
            .HasForeignKey(action => action.OrganizationId)
            .HasConstraintName("FK_ORGANIZATION");
        
        builder.Property(action => action.State)
            .IsRequired()
            .HasConversion(  
                s => s.ToString(), 
                s => (States)Enum.Parse(typeof(States), s));
        
        builder.Property(action => action.Interests)
            .IsRequired()
            .HasConversion(
                interests => string.Join(",", interests.Select(i => i.ToString())),
                interests => interests.Split(",", StringSplitOptions.None).Select(i => (Interests) Enum.Parse(typeof(Interests), i)).ToList());

    }
}