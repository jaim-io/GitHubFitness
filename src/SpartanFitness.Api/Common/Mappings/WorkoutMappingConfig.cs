using Mapster;

using SpartanFitness.Application.Workouts.Commands.CreateWorkout;
using SpartanFitness.Application.Workouts.Queries.GetWorkoutPage;
using SpartanFitness.Contracts.Common;
using SpartanFitness.Contracts.Workouts;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Models;
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

    config.NewConfig<PagingRequest, GetWorkoutPageQuery>()
      .Map(dest => dest.PageNumber, src => src.Page)
      .Map(dest => dest.PageSize, src => src.Size)
      .Map(dest => dest.SearchQuery, src => src.Query)
      .Map(dest => dest, src => src);

    config.NewConfig<Pagination<Workout>, WorkoutPageResponse>()
      .Map(dest => dest.Workouts, src => src.Content)
      .Map(dest => dest, src => src);

    config.NewConfig<Workout, WorkoutPageWorkoutResponse>()
      .Map(dest => dest.Id, src => src.Id.Value.ToString())
      .Map(dest => dest.CoachId, src => src.CoachId.Value.ToString())
      .Map(dest => dest.MuscleIds, src => src.MuscleIds.Select(muscleId => muscleId.Value.ToString()))
      .Map(dest => dest.MuscleGroupIds, src => src.MuscleGroupIds.Select(muscleGroupId => muscleGroupId.Value.ToString()));

    config.NewConfig<WorkoutExercise, WorkoutPageWorkoutExerciseResponse>()
      .Map(dest => dest.Id, src => src.Id.Value.ToString())
      .Map(dest => dest.MinReps, src => src.RepRange.MinReps)
      .Map(dest => dest.MaxReps, src => src.RepRange.MaxReps)
      .Map(dest => dest.ExerciseType, src => src.ExerciseType.Name);
  }
}