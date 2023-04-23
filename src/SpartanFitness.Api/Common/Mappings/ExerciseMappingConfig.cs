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

    config.NewConfig<PagingRequest, GetExercisePageQuery>()
      .Map(dest => dest.PageNumber, src => src.Page)
      .Map(dest => dest.PageSize, src => src.Size)
      .Map(dest => dest.Sort, src => src.Sort)
      .Map(dest => dest.SearchQuery, src => src.Search)
      .Map(dest => dest, src => src);

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