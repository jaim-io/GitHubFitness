using Mapster;

using SpartanFitness.Application.Workouts.Commands.CreateWorkout;
using SpartanFitness.Contracts.Workouts;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Entities;

namespace SpartanFitness.Api.Common.Mappings;

public class WorkoutMappingConfig : IRegister
{
  public void Register(TypeAdapterConfig config)
  {
    config.NewConfig<(CreateWorkoutRequest Request, string CoachId), CreateWorkoutCommand>()
      .Map(dest => dest.CoachId, src => src.CoachId)
      .Map(dest => dest, src => src.Request);

    config.NewConfig<Workout, WorkoutResponse>()
      .Map(dest => dest.Id, src => src.Id.Value.ToString())
      .Map(dest => dest.CoachId, src => src.CoachId.Value.ToString())
      .Map(dest => dest.MuscleIds, src => src.MuscleIds.Select(muscleId => muscleId.Value.ToString()))
      .Map(dest => dest.MuscleGroupIds, src => src.MuscleGroupIds.Select(muscleGroupId => muscleGroupId.Value.ToString()));

    config.NewConfig<WorkoutExercise, WorkoutExerciseResponse>()
      .Map(dest => dest.Id, src => src.Id.Value.ToString())
      .Map(dest => dest.MinReps, src => src.RepRange.MinReps)
      .Map(dest => dest.MaxReps, src => src.RepRange.MaxReps)
      .Map(dest => dest.ExerciseType, src => src.ExerciseType.Name);
  }
}