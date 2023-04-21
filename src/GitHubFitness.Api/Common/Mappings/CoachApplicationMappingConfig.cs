using GitHubFitness.Application.CoachApplications.Commands.ApproveCoachApplication;
using GitHubFitness.Application.CoachApplications.Commands.CreateCoachApplication;
using GitHubFitness.Application.CoachApplications.Commands.DenyCoachApplication;
using GitHubFitness.Contracts.CoachApplications;
using GitHubFitness.Domain.Aggregates;

using Mapster;

namespace GitHubFitness.Api.Common.Mappings;

public class CoachApplicationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(CreateCoachApplicationRequest Request, string UserId), CreateCoachApplicationCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest, src => src.Request);

        config.NewConfig<(ApproveCoachApplicationRequest Request, string UserId, string ApplicationId), ApproveCoachApplicationCommand>()
            .Map(dest => dest.Id, src => src.ApplicationId)
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest, src => src.Request);

        config.NewConfig<(DenyCoachApplicationRequest Request, string UserId, string ApplicationId), DenyCoachApplicationCommand>()
            .Map(dest => dest.Id, src => src.ApplicationId)
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest, src => src.Request);

        config.NewConfig<CoachApplication, CoachApplicationResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.UserId, src => src.UserId.Value)
            .Map(dest => dest, src => src);
    }
}