using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.Entities;
using GitHubFitness.Domain.Enums;
using GitHubFitness.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GitHubFitness.Infrastructure.Configuration;

public class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
{
  public void Configure(EntityTypeBuilder<Workout> builder)
  {
    ConfigureWorkoutsTable(builder);
    ConfigureWorkoutExerciseTable(builder);
    ConfigureWorkoutMuscleGroupIdsTable(builder);
  }

  private void ConfigureWorkoutMuscleGroupIdsTable(EntityTypeBuilder<Workout> builder)
  {
    builder.OwnsMany(we => we.MuscleGroupIds, mgi =>
    {
      mgi.ToTable("WorkoutMuscleGroupIds");

      mgi.WithOwner().HasForeignKey("WorkoutId");

      mgi.HasKey("Id");

      mgi.Property(mg => mg.Value)
        .HasColumnName("MuscleGroupId")
        .ValueGeneratedNever();
    });

    builder.Metadata.FindNavigation(nameof(Workout.MuscleGroupIds))!
      .SetPropertyAccessMode(PropertyAccessMode.Field);
  }

  private void ConfigureWorkoutExerciseTable(EntityTypeBuilder<Workout> builder)
  {
    builder.OwnsMany(w => w.WorkoutExercises, we =>
    {
      we.ToTable("WorkoutExercises");

      we.WithOwner().HasForeignKey("WorkoutId");

      we.HasKey(nameof(WorkoutExercise.Id), "WorkoutId");

      we.Property(we => we.Id)
        .HasColumnName("WorkoutExerciseId")
        .ValueGeneratedNever()
        .HasConversion(
          id => id.Value,
          value => WorkoutExerciseId.Create(value));

      we.Property(we => we.OrderNumber)
        .ValueGeneratedNever();

      we.Property(we => we.ExerciseId)
        .ValueGeneratedNever()
        .HasConversion(
          id => id.Value,
          value => ExerciseId.Create(value));

      we.OwnsOne(we => we.RepRange);

      we.Property(we => we.ExerciseType)
        .HasConversion(
          type => type.Id,
          value => (ExerciseType)value);
    });

    builder.Metadata.FindNavigation(nameof(Workout.WorkoutExercises))!
      .SetPropertyAccessMode(PropertyAccessMode.Field);
  }

  private void ConfigureWorkoutsTable(EntityTypeBuilder<Workout> builder)
  {
    builder.ToTable("Workouts");

    builder.HasKey(w => w.Id);

    builder.Property(w => w.Id)
      .ValueGeneratedNever()
      .HasConversion(
        id => id.Value,
        value => WorkoutId.Create(value));

    builder.Property(w => w.Name)
      .HasMaxLength(100);

    builder.Property(w => w.Description)
      .HasMaxLength(512);

    builder.Property(w => w.Image)
      .HasMaxLength(256);

    builder.Property(w => w.CoachId)
      .ValueGeneratedNever()
      .HasConversion(
        id => id.Value,
        value => CoachId.Create(value));
  }
}