using GitHubFitness.Application.Coaches.Commands.CreateCoach;
using GitHubFitness.Contracts.Coaches;
using GitHubFitness.Domain.Aggregates;

using Mapster;

namespace GitHubFitness.Api.Common.Mappings;

public class CoachMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateCoachRequest, CreateCoachCommand>();

        config.NewConfig<Coach, CoachResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.UserId, src => src.UserId.Value)
            .Map(dest => dest, src => src);
    }
}