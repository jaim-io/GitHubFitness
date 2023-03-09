using Mapster;

using SpartanFitness.Application.CoachApplications.Commands.ApproveCoachApplication;
using SpartanFitness.Application.CoachApplications.Commands.CreateCoachApplication;
using SpartanFitness.Application.CoachApplications.Commands.DenyCoachApplication;
using SpartanFitness.Application.CoachApplications.Common;
using SpartanFitness.Application.CoachApplications.Queries.GetCoachApplicationById;
using SpartanFitness.Contracts.CoachApplications;

namespace SpartanFitness.Api.Common.Mappings;

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

        config.NewConfig<GetCoachApplicationByIdRequest, GetCoachApplicationByIdQuery>();

        config.NewConfig<CoachApplicationResult, CoachApplicationResponse>()
            .Map(dest => dest.Id, src => src.CoachApplication.Id.Value)
            .Map(dest => dest.UserId, src => src.CoachApplication.UserId.Value)
            .Map(dest => dest, src => src.CoachApplication);
    }
}