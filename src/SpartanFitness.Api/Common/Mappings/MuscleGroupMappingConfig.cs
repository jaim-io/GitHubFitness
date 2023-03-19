using Mapster;

using SpartanFitness.Application.MuscleGroups.CreateMuscleGroup;
using SpartanFitness.Application.MuscleGroups.Queries.GetMuscleGroupById;
using SpartanFitness.Contracts.MuscleGroups;
using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Api.Common.Mappings;

public class MuscleGroupMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(CreateMuscleGroupRequest Request, string UserId), CreateMuscleGroupCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest, src => src.Request);

        config.NewConfig<GetMuscleGroupRequest, GetMuscleGroupByIdQuery>();

        config.NewConfig<MuscleGroup, MuscleGroupResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.CoachId, src => src.CreatorId.Value)
            .Map(dest => dest, src => src);
    }
}