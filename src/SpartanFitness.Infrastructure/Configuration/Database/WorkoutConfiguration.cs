using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Entities;
using SpartanFitness.Domain.Enums;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Configuration;

public class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
{
  public void Configure(EntityTypeBuilder<Workout> builder)
  {
    ConfigureWorkoutsTable(builder);
    ConfigureWorkoutExerciseTable(builder);
    ConfigureWorkoutMuscleIdsTable(builder);
    ConfigureWorkoutMuscleGroupIdsTable(builder);
  }

  private void ConfigureWorkoutMuscleIdsTable(EntityTypeBuilder<Workout> builder)
  {
    builder.OwnsMany(we => we.MuscleIds, mib =>
    {
      mib.ToTable("WorkoutMuscleIds");

      mib.WithOwner().HasForeignKey("WorkoutId");

      mib.HasKey("Id");

      mib.Property(mg => mg.Value)
        .HasColumnName("MuscleId")
        .ValueGeneratedNever();
    });

    builder.Metadata.FindNavigation(nameof(Workout.MuscleIds))!
      .SetPropertyAccessMode(PropertyAccessMode.Field);
  }

  private void ConfigureWorkoutMuscleGroupIdsTable(EntityTypeBuilder<Workout> builder)
  {
    builder.OwnsMany(we => we.MuscleGroupIds, mgib =>
    {
      mgib.ToTable("WorkoutMuscleGroupIds");

      mgib.WithOwner().HasForeignKey("WorkoutId");

      mgib.HasKey("Id");

      mgib.Property(mg => mg.Value)
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
      .HasMaxLength(2048);

    builder.Property(w => w.Image)
      .HasMaxLength(2048);

    builder.Property(w => w.CoachId)
      .ValueGeneratedNever()
      .HasConversion(
        id => id.Value,
        value => CoachId.Create(value));
  }
}