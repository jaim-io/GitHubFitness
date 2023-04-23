using Mapster;

using SpartanFitness.Application.Exercises.CreateExercise;
using SpartanFitness.Application.Exercises.Queries.GetExercisePage;
using SpartanFitness.Contracts.Common;
using SpartanFitness.Contracts.Exercises;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Api.Common.Mappings;

public class ExerciseMappingConfig : IRegister
{
  public void Register(TypeAdapterConfig config)
  {
    config.NewConfig<(CreateExerciseRequest Request, string UserId), CreateExerciseCommand>()
      .Map(dest => dest.UserId, src => src.UserId)
      .Map(dest => dest, src => src.Request);

    config.NewConfig<Exercise, ExerciseResponse>()
      .Map(dest => dest.Id, src => src.Id.Value)
      .Map(dest => dest.CreatorId, src => src.CreatorId.Value)
      .Map(dest => dest.MuscleGroupIds, src => src.MuscleGroupIds.Select(muscleGroupId => muscleGroupId.Value))
      .Map(dest => dest, src => src);

    config.NewConfig<(PagingRequest Request, ExerciseFilters Filters), GetExercisePageQuery>()
      .Map(dest => dest.PageNumber, src => src.Request.Page)
      .Map(dest => dest.PageSize, src => src.Request.Size)
      .Map(dest => dest.Sort, src => src.Request.Sort)
      .Map(dest => dest.SearchQuery, src => src.Request.Search)
      .Map(dest => dest, src => src.Filters);

    config.NewConfig<Page<Exercise>, ExercisePageResponse>()
      .Map(dest => dest.Exercises, src => src.Content)
      .Map(dest => dest, src => src);

    config.NewConfig<Exercise, ExercisePageExerciseResponse>()
      .Map(dest => dest.Id, src => src.Id.Value)
      .Map(dest => dest.CreatorId, src => src.CreatorId.Value)
      .Map(dest => dest.MuscleGroupIds, src => src.MuscleGroupIds.Select(muscleGroupId => muscleGroupId.Value))
      .Map(dest => dest, src => src);
  }
}