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
    Func<Exercise, bool>? filter = null;
    Guid guid;
    if (Guid.TryParse(query.SearchQuery, out guid))
    {
      var muscleGroupId = MuscleGroupId.Create(guid);
      var creatorId = CoachId.Create(guid);

      filter = (ex) => ex.CreatorId.Equals(creatorId) || ex.MuscleGroupIds.Contains(muscleGroupId);
    }
    else if (query.SearchQuery is not null)
    {
      filter = (ex) => ex.Name.ToLower().Contains(query.SearchQuery) || ex.Description.ToLower().Contains(query.SearchQuery);
    }

    var pageNumber = query.PageNumber ?? 1;

    IEnumerable<Exercise> skippedExercises;
    decimal pageCount;
    {
      IEnumerable<Exercise> exercises;
      if (filter != null)
      {
        exercises = _exerciseRepository.GetAllWithFilter(filter);
      }
      else
      {
        exercises = await _exerciseRepository.GetAllAsync();
      }

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
      "name" => query.Order switch
      {
        "asc" => content.OrderBy(ex => ex.Name),
        "desc" => content.OrderByDescending(ex => ex.Name),
        _ => content.OrderByDescending(ex => ex.Name),
      },
      "created" => query.Order switch
      {
        "newest" => content.OrderByDescending(ex => ex.CreatedDateTime),
        "oldest" => content.OrderBy(ex => ex.CreatedDateTime),
        _ => content.OrderByDescending(ex => ex.CreatedDateTime),
      },
      "updated" => query.Order switch
      {
        "newest" => content.OrderByDescending(ex => ex.UpdatedDateTime),
        "oldest" => content.OrderBy(ex => ex.UpdatedDateTime),
        _ => content.OrderByDescending(ex => ex.UpdatedDateTime),
      },
      _ => content.OrderByDescending(ex => ex.CreatedDateTime),
    };

    return new Page<Exercise>(
      content.ToList(),
      pageNumber,
      (int)pageCount);
  }
}