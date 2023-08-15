using Mapster;

using SpartanFitness.Application.Coaches.Commands.CreateCoach;
using SpartanFitness.Application.Coaches.Common;
using SpartanFitness.Contracts.Coaches;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Api.Common.Mappings;

public class CoachMappingConfig : IRegister
{
  public void Register(TypeAdapterConfig config)
  {
    config.NewConfig<CreateCoachRequest, CreateCoachCommand>();

    config.NewConfig<CoachResult, CoachResponse>()
      .Map(dest => dest.Id, src => src.Coach.Id.Value.ToString())
      .Map(dest => dest.UserId, src => src.Coach.UserId.Value.ToString())
      .Map(dest => dest.Biography, src => src.Coach.Biography)
      .Map(dest => dest.SocialMedia, src => src.Coach.SocialMedia)
      .Map(dest => dest.CreatedDateTime, src => src.Coach.CreatedDateTime)
      .Map(dest => dest.UpdatedDateTime, src => src.Coach.UpdatedDateTime)
      .Map(dest => dest, src => src.User);

    config.NewConfig<SocialMedia, CoachResponseSocialMedia>()
      .Map(dest => dest, src => src);
  }
}