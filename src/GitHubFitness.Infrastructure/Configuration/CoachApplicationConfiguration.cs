using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GitHubFitness.Infrastructure.Configuration;

public class CoachApplicationConfiguration : IEntityTypeConfiguration<CoachApplication>
{
    public void Configure(EntityTypeBuilder<CoachApplication> builder)
    {
        ConfigureCoachApplicationsTable(builder);
    }

    private void ConfigureCoachApplicationsTable(EntityTypeBuilder<CoachApplication> builder)
    {
        builder.ToTable("CoachApplications");

        builder.HasKey(ca => ca.Id);

        builder.Property(ca => ca.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => CoachApplicationId.Create(value));
        
        builder.Property(ca => ca.UserId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));

        builder.Property(ca => ca.Motivation)
            .HasMaxLength(255);
        
        builder.Property(ca => ca.Remarks)
            .HasMaxLength(255);
    }
}