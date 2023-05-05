using Mapster;

using SpartanFitness.Application.Exercises.Command.CreateExercise;
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
      .Map(dest => dest.Id, src => src.Id.Value.ToString())
      .Map(dest => dest.CreatorId, src => src.CreatorId.Value.ToString())
      .Map(dest => dest.MuscleGroupIds, src => src.MuscleGroupIds.Select(muscleGroupId => muscleGroupId.Value.ToString()))
      .Map(dest => dest.MuscleIds, src => src.MuscleIds.Select(muscleId => muscleId.Value.ToString()))
      .Map(dest => dest, src => src);

    config.NewConfig<PagingRequest, GetExercisePageQuery>()
      .Map(dest => dest.PageNumber, src => src.Page)
      .Map(dest => dest.PageSize, src => src.Size)
      .Map(dest => dest.SearchQuery, src => src.Query)
      .Map(dest => dest, src => src);

    config.NewConfig<Pagination<Exercise>, ExercisePageResponse>()
      .Map(dest => dest.Exercises, src => src.Content)
      .Map(dest => dest, src => src);

    config.NewConfig<Exercise, ExercisePageExerciseResponse>()
      .Map(dest => dest.Id, src => src.Id.Value.ToString())
      .Map(dest => dest.CreatorId, src => src.CreatorId.Value.ToString())
      .Map(dest => dest.MuscleGroupIds, src => src.MuscleGroupIds.Select(muscleGroupId => muscleGroupId.Value.ToString()))
      .Map(dest => dest.MuscleIds, src => src.MuscleIds.Select(muscleId => muscleId.Value.ToString()))
      .Map(dest => dest, src => src);
  }
}