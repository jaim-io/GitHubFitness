using Mapster;

using SpartanFitness.Application.Common.Results;
using SpartanFitness.Application.Users.Commands.SaveExercise;
using SpartanFitness.Application.Users.Commands.SaveMuscle;
using SpartanFitness.Application.Users.Commands.SaveMuscleGroup;
using SpartanFitness.Application.Users.Commands.SaveWorkout;
using SpartanFitness.Application.Users.Commands.UnSaveExercise;
using SpartanFitness.Application.Users.Commands.UnSaveMuscle;
using SpartanFitness.Application.Users.Commands.UnSaveMuscleGroup;
using SpartanFitness.Application.Users.Commands.UnSaveWorkout;
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

    config.NewConfig<(UnSaveExerciseRequest Request, string UserId), UnSaveExerciseCommand>()
      .Map(dest => dest.UserId, src => src.UserId)
      .Map(dest => dest.ExerciseId, src => src.Request.ExerciseId)
      .Map(dest => dest, src => src);

    config.NewConfig<(SaveMuscleGroupRequest Request, string UserId), SaveMuscleGroupCommand>()
      .Map(dest => dest.UserId, src => src.UserId)
      .Map(dest => dest.MuscleGroupId, src => src.Request.MuscleGroupId)
      .Map(dest => dest, src => src);

    config.NewConfig<(UnSaveMuscleGroupRequest Request, string UserId), UnSaveMuscleGroupCommand>()
      .Map(dest => dest.UserId, src => src.UserId)
      .Map(dest => dest.MuscleGroupId, src => src.Request.MuscleGroupId)
      .Map(dest => dest, src => src);

    config.NewConfig<(SaveMuscleRequest Request, string UserId), SaveMuscleCommand>()
      .Map(dest => dest.UserId, src => src.UserId)
      .Map(dest => dest.MuscleId, src => src.Request.MuscleId)
      .Map(dest => dest, src => src);

    config.NewConfig<(UnSaveMuscleRequest Request, string UserId), UnSaveMuscleCommand>()
      .Map(dest => dest.UserId, src => src.UserId)
      .Map(dest => dest.MuscleId, src => src.Request.MuscleId)
      .Map(dest => dest, src => src);

    config.NewConfig<(SaveWorkoutRequest Request, string UserId), SaveWorkoutCommand>()
      .Map(dest => dest.UserId, src => src.UserId)
      .Map(dest => dest.WorkoutId, src => src.Request.WorkoutId)
      .Map(dest => dest, src => src);

    config.NewConfig<(UnSaveWorkoutRequest Request, string UserId), UnSaveWorkoutCommand>()
      .Map(dest => dest.UserId, src => src.UserId)
      .Map(dest => dest.WorkoutId, src => src.Request.WorkoutId)
      .Map(dest => dest, src => src);

    config.NewConfig<UserSavesResult, UserSavesResponse>();
  }
}