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
    ConfigureExerciseMuscleIdsTable(builder);
  }

  private void ConfigureExerciseMuscleIdsTable(EntityTypeBuilder<Exercise> builder)
  {
    builder.OwnsMany(e => e.MuscleIds, mi =>
    {
      mi.ToTable("ExerciseMuscleIds");

      mi.WithOwner().HasForeignKey("ExerciseId");

      mi.HasKey("Id");

      mi.Property(m => m.Value)
        .HasColumnName("MuscleId")
        .ValueGeneratedNever();
    });

    builder.Metadata.FindNavigation(nameof(Exercise.MuscleIds))!
      .SetPropertyAccessMode(PropertyAccessMode.Field);
  }

  private void ConfigureExerciseMuscleGroupIdsTable(EntityTypeBuilder<Exercise> builder)
  {
    builder.OwnsMany(e => e.MuscleGroupIds, mgib =>
    {
      mgib.ToTable("ExerciseMuscleGroupIds");

      mgib.WithOwner().HasForeignKey("ExerciseId");

      mgib.HasKey("Id");

      mgib.Property(mgi => mgi.Value)
        .ValueGeneratedNever()
        .HasColumnName("MuscleGroupId");
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
      .HasMaxLength(2048);

    builder.Property(e => e.Image)
      .HasMaxLength(2048);

    builder.Property(e => e.Video)
      .HasMaxLength(2048);

    builder.Property(e => e.CreatorId)
      .ValueGeneratedNever()
      .HasConversion(
        id => id.Value,
        value => CoachId.Create(value));

    builder.Property(e => e.LastUpdaterId)
      .ValueGeneratedNever()
      .HasConversion(
      id => id.Value,
      value => CoachId.Create(value));
  }
}