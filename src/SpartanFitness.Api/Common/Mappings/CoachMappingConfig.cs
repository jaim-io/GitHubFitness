using Mapster;

using SpartanFitness.Application.Coaches.Commands.CreateCoach;
using SpartanFitness.Application.Coaches.Common;
using SpartanFitness.Application.Coaches.Queries.GetCoachById;
using SpartanFitness.Contracts.Coaches;

namespace SpartanFitness.Api.Common.Mappings;

public class CoachMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateCoachRequest, CreateCoachCommand>();

        config.NewConfig<GetCoachRequest, GetCoachByIdQuery>();

        config.NewConfig<CoachResult, CoachResponse>()
            .Map(dest => dest.Id, src => src.Coach.Id.Value)
            .Map(dest => dest.UserId, src => src.Coach.UserId.Value)
            .Map(dest => dest, src => src.Coach);
    }
}