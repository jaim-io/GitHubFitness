using GitHubFitness.Application.Workouts.Commands.CreateWorkout;
using GitHubFitness.Contracts.Workouts;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.Entities;

using Mapster;

namespace GitHubFitness.Api.Common.Mappings;

public class WorkoutMappingConfig : IRegister
{
  public void Register(TypeAdapterConfig config)
  {
    config.NewConfig<(CreateWorkoutRequest Request, string CoachId), CreateWorkoutCommand>()
      .Map(dest => dest.CoachId, src => src.CoachId)
      .Map(dest => dest, src => src.Request);

    config.NewConfig<Workout, WorkoutResponse>()
      .Map(dest => dest.Id, src => src.Id.Value)
      .Map(dest => dest.CoachId, src => src.CoachId.Value)
      .Map(dest => dest.MuscleGroupIds, src => src.MuscleGroupIds.Select(muscleGroupId => muscleGroupId.Value));

    config.NewConfig<WorkoutExercise, WorkoutExerciseResponse>()
      .Map(dest => dest.Id, src => src.Id.Value)
      .Map(dest => dest.MinReps, src => src.RepRange.MinReps)
      .Map(dest => dest.MaxReps, src => src.RepRange.MaxReps)
      .Map(dest => dest.ExerciseType, src => src.ExerciseType.Name);
  }
}