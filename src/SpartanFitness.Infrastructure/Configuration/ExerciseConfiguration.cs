using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Configuration;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        ConfigureExercisesTable(builder);
        ConfigureExerciseMuscleGroupIdsTable(builder);
    }

    private void ConfigureExerciseMuscleGroupIdsTable(EntityTypeBuilder<Exercise> builder)
    {
        builder.OwnsMany(e => e.MuscleGroupIds, mgi =>
        {
            mgi.ToTable("ExerciseMuscleGroupIds");

            mgi.WithOwner().HasForeignKey("ExerciseId");

            mgi.HasKey("Id");

            mgi.Property(mg => mg.Value)
                .HasColumnName("MuscleGroupId")
                .ValueGeneratedNever();
        });

        builder.Metadata.FindNavigation(nameof(Exercise.MuscleGroupIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private void ConfigureExercisesTable(EntityTypeBuilder<Exercise> builder)
    {
        builder.ToTable("Exercises");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => ExerciseId.Create(value));

        builder.Property(e => e.Name)
            .HasMaxLength(100);

        builder.Property(e => e.Description)
            .HasMaxLength(512);

        builder.Property(e => e.Image)
            .HasMaxLength(512);

        builder.Property(e => e.Video)
            .HasMaxLength(512);

        builder.Property(e => e.CreatorId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => CoachId.Create(value));
    }
}