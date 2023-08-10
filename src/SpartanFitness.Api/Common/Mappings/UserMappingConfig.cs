using Mapster;

using SpartanFitness.Application.Common.Results;
using SpartanFitness.Application.Users.Commands.SaveExercise;
using SpartanFitness.Application.Users.Commands.SaveMuscle;
using SpartanFitness.Application.Users.Commands.SaveMuscleGroup;
using SpartanFitness.Application.Users.Commands.SaveWorkout;
using SpartanFitness.Application.Users.Queries.GetSavedExercisePage;
using SpartanFitness.Contracts.Common;
using SpartanFitness.Contracts.Users;
using SpartanFitness.Contracts.Users.Saves;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Api.Common.Mappings;

public class UserMappingConfig : IRegister
{
  public void Register(TypeAdapterConfig config)
  {
    config.NewConfig<User, UserResponse>()
      .Map(dest => dest.Id, src => src.Id.Value.ToString())
      .Map(dest => dest.SavedExerciseIds, src => src.SavedExerciseIds.Select(id => id.Value.ToString()))
      .Map(dest => dest.SavedMuscleIds, src => src.SavedMuscleIds.Select(id => id.Value.ToString()))
      .Map(dest => dest.SavedMuscleGroupIds, src => src.SavedMuscleGroupIds.Select(id => id.Value.ToString()))
      .Map(dest => dest.SavedWorkoutIds, src => src.SavedWorkoutIds.Select(id => id.Value.ToString()))
      .Map(dest => dest, src => src);

    config.NewConfig<(SaveExerciseRequest Request, string UserId), SaveExerciseCommand>()
      .Map(dest => dest.UserId, src => src.UserId)
      .Map(dest => dest.ExerciseId, src => src.Request.ExerciseId)
      .Map(dest => dest, src => src);

    config.NewConfig<(SaveMuscleGroupRequest Request, string UserId), SaveMuscleGroupCommand>()
      .Map(dest => dest.UserId, src => src.UserId)
      .Map(dest => dest.MuscleGroupId, src => src.Request.MuscleGroupId)
      .Map(dest => dest, src => src);

    config.NewConfig<(SaveMuscleRequest Request, string UserId), SaveMuscleCommand>()
      .Map(dest => dest.UserId, src => src.UserId)
      .Map(dest => dest.MuscleId, src => src.Request.MuscleId)
      .Map(dest => dest, src => src);

    config.NewConfig<(SaveWorkoutRequest Request, string UserId), SaveWorkoutCommand>()
      .Map(dest => dest.UserId, src => src.UserId)
      .Map(dest => dest.WorkoutId, src => src.Request.WorkoutId)
      .Map(dest => dest, src => src);

    config.NewConfig<UserSavesResult, UserSavesResponse>();

    config.NewConfig<List<ExerciseId>, SavedExerciseIdsResponse>()
      .Map(dest => dest.Ids, src => src.Select(id => id.Value.ToString()));

    config.NewConfig<List<MuscleGroupId>, SavedMuscleGroupIdsResponse>()
      .Map(dest => dest.Ids, src => src.Select(id => id.Value.ToString()));

    config.NewConfig<List<MuscleId>, SavedMuscleIdsResponse>()
      .Map(dest => dest.Ids, src => src.Select(id => id.Value.ToString()));

    config.NewConfig<List<WorkoutId>, SavedWorkoutIdsResponse>()
      .Map(dest => dest.Ids, src => src.Select(id => id.Value.ToString()));

    config.NewConfig<(PagingRequest Request, string UserId), GetSavedExercisePageQuery>()
      .Map(dest => dest.UserId, src => src.UserId)
      .Map(dest => dest.PageNumber, src => src.Request.Page)
      .Map(dest => dest.PageSize, src => src.Request.Size)
      .Map(dest => dest.SearchQuery, src => src.Request.Query)
      .Map(dest => dest, src => src.Request);
  }
}