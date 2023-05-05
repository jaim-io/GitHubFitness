using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Application.Exercises.Queries.GetExercisePage;

public class GetExercisePageQueryHandler
  : IRequestHandler<GetExercisePageQuery, ErrorOr<Pagination<Exercise>>>
{
  private readonly IExerciseRepository _exerciseRepository;

  public GetExercisePageQueryHandler(
    IExerciseRepository exerciseRepository)
  {
    _exerciseRepository = exerciseRepository;
  }

  public async Task<ErrorOr<Pagination<Exercise>>> Handle(
    GetExercisePageQuery query,
    CancellationToken cancellationToken)
  {
    var pageNumber = query.PageNumber ?? 1;

    IEnumerable<Exercise> exercises = query.SearchQuery is not null
      ? await _exerciseRepository.GetBySearchQueryAsync(query.SearchQuery)
      : await _exerciseRepository.GetAllAsync();

    exercises = query.Sort switch
    {
      "name" => query.Order switch
      {
        "asc" => exercises.OrderBy(ex => ex.Name),
        "desc" => exercises.OrderByDescending(ex => ex.Name),
        _ => exercises.OrderByDescending(ex => ex.Name),
      },
      "created" => query.Order switch
      {
        "desc" => exercises.OrderByDescending(ex => ex.CreatedDateTime),
        "asc" => exercises.OrderBy(ex => ex.CreatedDateTime),
        _ => exercises.OrderByDescending(ex => ex.CreatedDateTime),
      },
      "updated" => query.Order switch
      {
        "desc" => exercises.OrderByDescending(ex => ex.UpdatedDateTime),
        "asc" => exercises.OrderBy(ex => ex.UpdatedDateTime),
        _ => exercises.OrderByDescending(ex => ex.UpdatedDateTime),
      },
      _ => exercises.OrderByDescending(ex => ex.CreatedDateTime),
    };

    exercises = exercises
      .Skip((pageNumber - 1) * query.PageSize ?? 0)
      .ToList();

    decimal pageCount = query.PageSize == null
      ? 1
      : Math.Ceiling((decimal)exercises.Count() / (int)query.PageSize);

    if (!(pageNumber == 1 && pageCount == 0) &&
        pageNumber > pageCount)
    {
      return Errors.Page.NotFound;
    }

    exercises = query.PageSize == null
      ? exercises
      : exercises
        .Take((int)query.PageSize);

    return new Pagination<Exercise>(
      exercises.ToList(),
      pageNumber,
      (int)pageCount);
  }
}