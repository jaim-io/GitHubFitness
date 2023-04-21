using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GitHubFitness.Infrastructure.Configuration;

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

        builder.Property(mg => mg.CreatorId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => CoachId.Create(value));
    }
}