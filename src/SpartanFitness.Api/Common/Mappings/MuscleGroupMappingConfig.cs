using Mapster;

using SpartanFitness.Application.MuscleGroups.Common;
using SpartanFitness.Application.MuscleGroups.CreateMuscleGroup;
using SpartanFitness.Contracts.MuscleGroups;

namespace SpartanFitness.Api.Common.Mappings;

public class MuscleGroupMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(CreateMuscleGroupRequest Request, string UserId), CreateMuscleGroupCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest, src => src.Request);

        config.NewConfig<MuscleGroupResult, MuscleGroupResponse>()
            .Map(dest => dest.Id, src => src.MuscleGroup.Id.Value)
            .Map(dest => dest.CoachId, src => src.MuscleGroup.CreatorId.Value)
            .Map(dest => dest, src => src.MuscleGroup);
    }
}