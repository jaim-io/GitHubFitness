using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Exercises.Queries.GetExercisePage;

public class GetExercisePageQueryHandler
  : IRequestHandler<GetExercisePageQuery, ErrorOr<Page<Exercise>>>
{
  private readonly IExerciseRepository _exerciseRepository;

  public GetExercisePageQueryHandler(
    IExerciseRepository exerciseRepository)
  {
    _exerciseRepository = exerciseRepository;
  }

  public async Task<ErrorOr<Page<Exercise>>> Handle(
    GetExercisePageQuery query,
    CancellationToken cancellationToken)
  {
    await Task.CompletedTask;

    Func<Exercise, bool>? searchQuery = null;
    Guid guid;
    if (Guid.TryParse(query.SearchQuery, out guid))
    {
      var muscleGroupId = MuscleGroupId.Create(guid);
      var creatorId = CoachId.Create(guid);

      searchQuery = (ex) => ex.CreatorId.Equals(creatorId) || ex.MuscleGroupIds.Contains(muscleGroupId);
    }
    else if (query.SearchQuery is not null)
    {
      searchQuery = (ex) => ex.Name.ToLower().Contains(query.SearchQuery) || ex.Description.ToLower().Contains(query.SearchQuery);
    }

    var pageNumber = query.PageNumber ?? 1;

    IEnumerable<Exercise> skippedExercises;
    decimal pageCount;
    {
      var exercises = _exerciseRepository.GetAll(searchQuery ?? null);

      skippedExercises = exercises.Skip((pageNumber - 1) * query.PageSize ?? 0);
      pageCount = query.PageSize == null
        ? 1
        : Math.Ceiling((decimal)exercises.Count() / (int)query.PageSize);
    }

    if (pageNumber > pageCount)
    {
      return Errors.Page.NotFound;
    }

    var content = query.PageSize == null
      ? skippedExercises
      : skippedExercises
          .Take((int)query.PageSize);

    content = query.Sort switch
    {
      "name_asc" => content.OrderBy(ex => ex.Name),
      "name_desc" => content.OrderByDescending(ex => ex.Name),
      "created_newest" => content.OrderByDescending(ex => ex.CreatedDateTime),
      "created_oldest" => content.OrderBy(ex => ex.CreatedDateTime),
      "updated_newest" => content.OrderByDescending(ex => ex.UpdatedDateTime),
      "updated_oldest" => content.OrderBy(ex => ex.UpdatedDateTime),
      _ => content.OrderByDescending(ex => ex.CreatedDateTime),
    };

    return new Page<Exercise>(
      content.ToList(),
      pageNumber,
      (int)pageCount);
  }
}