using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Configuration;

public class MuscleGroupConfiguration : IEntityTypeConfiguration<MuscleGroup>
{
    public void Configure(EntityTypeBuilder<MuscleGroup> builder)
    {
        ConfigureMuscleGroupTable(builder);
    }

    private void ConfigureMuscleGroupTable(EntityTypeBuilder<MuscleGroup> builder)
    {
        builder.ToTable("MuscleGroups");

        builder.HasKey(mg => mg.Id);

        builder.Property(mg => mg.Id)
            .HasConversion(
                id => id.Value,
                value => MuscleGroupId.Create(value));

        builder.Property(mg => mg.CreatorId)
            .HasConversion(
                id => id.Value,
                value => CoachId.Create(value));
    }
}