using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Application.Workouts.Queries.GetWorkoutPage;

public class GetWorkoutPageQueryHandler
  : IRequestHandler<GetWorkoutPageQuery, ErrorOr<Pagination<Workout>>>
{
  private readonly IWorkoutRepository _workoutRepository;

  public GetWorkoutPageQueryHandler(IWorkoutRepository workoutRepository)
  {
    _workoutRepository = workoutRepository;
  }

  public async Task<ErrorOr<Pagination<Workout>>> Handle(GetWorkoutPageQuery query, CancellationToken cancellationToken)
  {
    var pageNumber = query.PageNumber ?? 1;

    IEnumerable<Workout> workouts = query.SearchQuery is not null
      ? await _workoutRepository.GetBySearchQueryAsync(query.SearchQuery)
      : await _workoutRepository.GetAllAsync();

    workouts = query.Sort switch
    {
      "name" => query.Order switch
      {
        "asc" => workouts.OrderBy(w => w.Name),
        "desc" => workouts.OrderByDescending(w => w.Name),
        _ => workouts.OrderByDescending(w => w.Name),
      },
      "created" => query.Order switch
      {
        "desc" => workouts.OrderByDescending(w => w.CreatedDateTime),
        "asc" => workouts.OrderBy(w => w.CreatedDateTime),
        _ => workouts.OrderByDescending(w => w.CreatedDateTime),
      },
      "updated" => query.Order switch
      {
        "desc" => workouts.OrderByDescending(w => w.UpdatedDateTime),
        "asc" => workouts.OrderBy(w => w.UpdatedDateTime),
        _ => workouts.OrderByDescending(w => w.UpdatedDateTime),
      },
      _ => workouts.OrderByDescending(w => w.CreatedDateTime),
    };

    decimal pageCount = query.PageSize == null
      ? 1
      : Math.Ceiling((decimal)workouts.Count() / (int)query.PageSize);

    workouts = workouts
     .Skip((pageNumber - 1) * query.PageSize ?? 0)
     .ToList();

    if (!(pageNumber == 1 && pageCount == 0) &&
        pageNumber > pageCount)
    {
      return Errors.Page.NotFound;
    }

    workouts = query.PageSize == null
      ? workouts
      : workouts
        .Take((int)query.PageSize);

    return new Pagination<Workout>(
      workouts.ToList(),
      pageNumber,
      (int)pageCount);
  }
}