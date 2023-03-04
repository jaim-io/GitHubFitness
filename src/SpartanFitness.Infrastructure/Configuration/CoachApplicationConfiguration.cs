using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Configuration;

public class CoachApplicationConfiguration : IEntityTypeConfiguration<CoachApplication>
{
    public void Configure(EntityTypeBuilder<CoachApplication> builder)
    {
        ConfigureCoachApplicationTable(builder);
    }

    private void ConfigureCoachApplicationTable(EntityTypeBuilder<CoachApplication> builder)
    {
        builder.ToTable("CoachApplications");

        builder.HasKey(ca => ca.Id);

        builder.Property(ca => ca.Id)
            .HasConversion(
                id => id.Value,
                value => CoachApplicationId.Create(value));
        
        builder.Property(ca => ca.UserId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));

        builder.Property(ca => ca.Motivation)
            .HasMaxLength(255);
        
        builder.Property(ca => ca.Remarks)
            .HasMaxLength(255);
    }
}