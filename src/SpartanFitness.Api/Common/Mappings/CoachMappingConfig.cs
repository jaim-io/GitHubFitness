using Mapster;

using SpartanFitness.Application.Coaches.Commands.CreateCoach;
using SpartanFitness.Application.Coaches.Common;
using SpartanFitness.Contracts.Coaches;

namespace SpartanFitness.Api.Common.Mappings;

public class CoachMappingConfig : IRegister
{
  public void Register(TypeAdapterConfig config)
  {
    config.NewConfig<CreateCoachRequest, CreateCoachCommand>();

    config.NewConfig<CoachResult, CoachResponse>()
      .Map(dest => dest.Id, src => src.Coach.Id.Value.ToString())
      .Map(dest => dest.UserId, src => src.Coach.UserId.Value.ToString())
      .Map(dest => dest.CreatedDateTime, src => src.Coach.CreatedDateTime)
      .Map(dest => dest.UpdatedDateTime, src => src.Coach.UpdatedDateTime)
      .Map(dest => dest, src => src.User);
  }
}