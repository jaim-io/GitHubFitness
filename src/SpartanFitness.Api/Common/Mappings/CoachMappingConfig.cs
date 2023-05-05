using Mapster;

using SpartanFitness.Application.Coaches.Commands.CreateCoach;
using SpartanFitness.Contracts.Coaches;
using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Api.Common.Mappings;

public class CoachMappingConfig : IRegister
{
  public void Register(TypeAdapterConfig config)
  {
    config.NewConfig<CreateCoachRequest, CreateCoachCommand>();

    config.NewConfig<Coach, CoachResponse>()
      .Map(dest => dest.Id, src => src.Id.Value.ToString())
      .Map(dest => dest.UserId, src => src.UserId.Value.ToString())
      .Map(dest => dest, src => src);
  }
}