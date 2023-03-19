using Mapster;

using SpartanFitness.Application.Coaches.Commands.CreateCoach;
using SpartanFitness.Application.Coaches.Queries.GetCoachById;
using SpartanFitness.Contracts.Coaches;
using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Api.Common.Mappings;

public class CoachMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateCoachRequest, CreateCoachCommand>();

        config.NewConfig<GetCoachRequest, GetCoachByIdQuery>();

        config.NewConfig<Coach, CoachResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.UserId, src => src.UserId.Value)
            .Map(dest => dest, src => src);
    }
}