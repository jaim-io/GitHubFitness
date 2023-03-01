using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Configuration;

public class CoachConfiguration : IEntityTypeConfiguration<Coach>
{
    public void Configure(EntityTypeBuilder<Coach> builder)
    {
        ConfigureCoachTable(builder);
    }

    private void ConfigureCoachTable(EntityTypeBuilder<Coach> builder)
    {
        builder.ToTable("Coaches");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => CoachId.Create(value));

        builder.Property(c => c.FirstName)
            .HasMaxLength(100);

        builder.Property(c => c.LastName)
            .HasMaxLength(100);

        builder.Property(c => c.ProfileImage)
            .HasMaxLength(250);

        builder.Property(c => c.UserId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));
    }
}