using Mapster;

using SpartanFitness.Contracts.Users;
using SpartanFitness.Domain.Aggregates;

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
  }
}