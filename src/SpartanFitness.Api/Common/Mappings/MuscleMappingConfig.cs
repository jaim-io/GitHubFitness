using Mapster;

using SpartanFitness.Application.Muscles.Command.CreateMuscle;
using SpartanFitness.Contracts.Muscles;
using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Api.Common.Mappings;

public class MuscleMappingConfig : IRegister
{
  public void Register(TypeAdapterConfig config)
  {
    config.NewConfig<CreateMuscleRequest, CreateMuscleCommand>()
      .Map(dest => dest, src => src);

    config.NewConfig<Muscle, MuscleResponse>()
      .Map(dest => dest.Id, src => src.Id.Value)
      .Map(dest => dest.MuscleGroupId, src => src.MuscleGroupId.Value)
      .Map(dest => dest, src => src);
  }
}