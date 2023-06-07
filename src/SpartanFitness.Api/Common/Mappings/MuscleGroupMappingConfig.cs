using Mapster;

using SpartanFitness.Application.MuscleGroups.Commands.CreateMuscleGroup;
using SpartanFitness.Application.MuscleGroups.Commands.UpdateMuscleGroup;
using SpartanFitness.Application.MuscleGroups.Queries.GetMuscleGroupPage;
using SpartanFitness.Contracts.Common;
using SpartanFitness.Contracts.MuscleGroups;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Api.Common.Mappings;

public class MuscleGroupMappingConfig : IRegister
{
  public void Register(TypeAdapterConfig config)
  {
    config.NewConfig<(CreateMuscleGroupRequest Request, string UserId), CreateMuscleGroupCommand>()
      .Map(dest => dest, src => src.Request);

    config.NewConfig<UpdateMuscleGroupRequest, UpdateMuscleGroupCommand>()
      .Map(dest => dest, src => src);

    config.NewConfig<MuscleGroup, MuscleGroupResponse>()
      .Map(dest => dest.Id, src => src.Id.Value.ToString())
      .Map(dest => dest.MuscleIds, src => src.MuscleIds.Select(muscleGroupId => muscleGroupId.Value.ToString()))
      .Map(dest => dest, src => src);

    config.NewConfig<PagingRequest, GetMuscleGroupPageQuery>()
      .Map(dest => dest.PageNumber, src => src.Page)
      .Map(dest => dest.PageSize, src => src.Size)
      .Map(dest => dest.SearchQuery, src => src.Query)
      .Map(dest => dest, src => src);

    config.NewConfig<Pagination<MuscleGroup>, MuscleGroupPageResponse>()
      .Map(dest => dest.MuscleGroups, src => src.Content)
      .Map(dest => dest, src => src);

    config.NewConfig<MuscleGroup, MuscleGroupPageMusclesResponse>()
      .Map(dest => dest.Id, src => src.Id.Value.ToString())
      .Map(dest => dest.MuscleIds, src => src.MuscleIds.Select(muscleGroupId => muscleGroupId.Value.ToString()))
      .Map(dest => dest, src => src);
  }
}