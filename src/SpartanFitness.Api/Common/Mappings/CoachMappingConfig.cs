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

        config.NewConfig<CoachResult, CoachReponse>()
            .Map(dest => dest.Id, src => src.Coach.Id.Value)
            .Map(dest => dest.UserId, src => src.Coach.UserId.Value)
            .Map(dest => dest, src => src.Coach);
    }
}