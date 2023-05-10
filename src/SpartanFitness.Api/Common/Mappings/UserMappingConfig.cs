using Mapster;

using SpartanFitness.Application.Users.Commands.SaveExercise;
using SpartanFitness.Application.Users.Commands.UnSaveExercise;
using SpartanFitness.Contracts.Users;
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
      .Map(dest => dest, src => src);

    config.NewConfig<(SaveExerciseRequest Request, string UserId), SaveExerciseCommand>()
      .Map(dest => dest.UserId, src => src.UserId)
      .Map(dest => dest.ExerciseId, src => src.Request.ExerciseId)
      .Map(dest => dest, src => src);

    config.NewConfig<(UnSaveExerciseRequest Request, string UserId), UnSaveExerciseCommand>()
      .Map(dest => dest.UserId, src => src.UserId)
      .Map(dest => dest.ExerciseId, src => src.Request.ExerciseId)
      .Map(dest => dest, src => src);
  }
}