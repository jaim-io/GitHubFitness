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
      .Map(dest => dest, src => src);

    config.NewConfig<(string UserId, string ExerciseId), SaveExerciseCommand>()
      .Map(dest => dest, src => src);
    
    config.NewConfig<(string UserId, string ExerciseId), UnSaveExerciseCommand>()
      .Map(dest => dest, src => src);
  }
}