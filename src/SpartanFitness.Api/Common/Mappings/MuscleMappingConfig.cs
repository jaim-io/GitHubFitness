using Mapster;

using SpartanFitness.Application.Muscles.Command.CreateMuscle;
using SpartanFitness.Application.Muscles.Query.GetMusclePage;
using SpartanFitness.Application.Muscles.Query.GetMusclePageByMuscleGroup;
using SpartanFitness.Contracts.Common;
using SpartanFitness.Contracts.Muscles;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Api.Common.Mappings;

public class MuscleMappingConfig : IRegister
{
  public void Register(TypeAdapterConfig config)
  {
    config.NewConfig<CreateMuscleRequest, CreateMuscleCommand>()
      .Map(dest => dest, src => src);

    config.NewConfig<Muscle, MuscleResponse>()
      .Map(dest => dest.Id, src => src.Id.Value)
      .Map(dest => dest.MuscleGroupId, src => src.MuscleGroupId.Value)
      .Map(dest => dest, src => src);

    config.NewConfig<PagingRequest, GetMusclePageQuery>()
      .Map(dest => dest.PageNumber, src => src.Page)
      .Map(dest => dest.PageSize, src => src.Size)
      .Map(dest => dest.SearchQuery, src => src.Query)
      .Map(dest => dest, src => src);

    config.NewConfig<(PagingRequest Request, string MuscleGroupId), GetMusclePageByMuscleGroupQuery>()
      .Map(dest => dest.MuscleGroupId, src => src.MuscleGroupId)
      .Map(dest => dest.PageNumber, src => src.Request.Page)
      .Map(dest => dest.PageSize, src => src.Request.Size)
      .Map(dest => dest.SearchQuery, src => src.Request.Query)
      .Map(dest => dest, src => src.Request);

    config.NewConfig<Page<Muscle>, MusclePageResponse>()
      .Map(dest => dest.Muscles, src => src.Content)
      .Map(dest => dest, src => src);

    config.NewConfig<Muscle, MusclePageMuscleResponse>()
      .Map(dest => dest.Id, src => src.Id.Value)
      .Map(dest => dest, src => src);
  }
}