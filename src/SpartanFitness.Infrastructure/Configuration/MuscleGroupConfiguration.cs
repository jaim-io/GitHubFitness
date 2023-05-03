using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Configuration;

public class MuscleGroupConfiguration : IEntityTypeConfiguration<MuscleGroup>
{
    public void Configure(EntityTypeBuilder<MuscleGroup> builder)
    {
        ConfigureMuscleGroupsTable(builder);
    }

    private void ConfigureMuscleGroupsTable(EntityTypeBuilder<MuscleGroup> builder)
    {
        builder.ToTable("MuscleGroups");

        builder.HasKey(mg => mg.Id);

        builder.Property(mg => mg.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => MuscleGroupId.Create(value));
    }
}