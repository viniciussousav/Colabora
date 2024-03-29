﻿using Colabora.Domain.Shared.Enums;
using Colabora.Domain.Volunteer;
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
        builder.Property(volunteer => volunteer.Email).IsRequired();
        
        builder.Property(volunteer => volunteer.FirstName).IsRequired();
        builder.Property(volunteer => volunteer.LastName).IsRequired();
        builder.Property(volunteer => volunteer.Birthdate).IsRequired();

        builder.Property(volunteer => volunteer.Gender)
            .IsRequired()
            .HasConversion(  
                g => g.ToString(), 
                g => (Gender)Enum.Parse(typeof(Gender), g));

        builder.Property(volunteer => volunteer.State)
            .IsRequired()
            .HasConversion(  
                s => s.ToString(), 
                s => (States)Enum.Parse(typeof(States), s));

        builder.Property(volunteer => volunteer.Interests)
            .IsRequired()
            .HasConversion(
                interests => string.Join(",", interests.Select(i => i.ToString())),
                interests => 
                    string.IsNullOrWhiteSpace(interests)
                    ? new List<Interests>()
                    : interests.Split(",", StringSplitOptions.None).Select(i => (Interests) Enum.Parse(typeof(Interests), i)).ToList());

        builder
            .HasMany(volunteer => volunteer.Participations)
            .WithOne(participation => participation.Volunteer)
            .HasForeignKey(participation => participation.VolunteerId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(volunteer => volunteer.CreateAt).HasDefaultValueSql("NOW()");
    }
}