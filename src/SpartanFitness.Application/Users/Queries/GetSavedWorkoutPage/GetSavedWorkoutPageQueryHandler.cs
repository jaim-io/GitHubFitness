using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Application.Users.Queries.GetSavedMusclePage;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Users.Queries.GetSavedWorkoutPage;

public class GetSavedWorkoutPageQueryHandler
  : IRequestHandler<GetSavedWorkoutPageQuery, ErrorOr<Pagination<Workout>>>
{
  private readonly IUserRepository _userRepository;
  private readonly IWorkoutRepository _workoutRepository;

  public GetSavedWorkoutPageQueryHandler(
    IUserRepository userRepository,
    IWorkoutRepository workoutRepository)
  {
    _userRepository = userRepository;
    _workoutRepository = workoutRepository;
  }

  public async Task<ErrorOr<Pagination<Workout>>> Handle(
    GetSavedWorkoutPageQuery query,
    CancellationToken cancellationToken)
  {
    var userId = UserId.Create(query.UserId);
    if (await _userRepository.GetByIdAsync(userId) is not User user)
    {
      return Errors.User.NotFound;
    }

    var workoutIds = user.SavedWorkoutIds.ToList();
    IEnumerable<Workout> workouts = query.SearchQuery is not null
      ? await _workoutRepository.GetBySearchQueryAndIdsAsync(query.SearchQuery, workoutIds)
      : await _workoutRepository.GetByIdAsync(workoutIds);

    var pageNumber = query.PageNumber ?? 1;

    workouts = query.Sort switch
    {
      "name" => query.Order switch
      {
        "asc" => workouts.OrderBy(ex => ex.Name),
        "desc" => workouts.OrderByDescending(ex => ex.Name),
        _ => workouts.OrderByDescending(ex => ex.Name),
      },
      "created" => query.Order switch
      {
        "desc" => workouts.OrderByDescending(ex => ex.CreatedDateTime),
        "asc" => workouts.OrderBy(ex => ex.CreatedDateTime),
        _ => workouts.OrderByDescending(ex => ex.CreatedDateTime),
      },
      "updated" => query.Order switch
      {
        "desc" => workouts.OrderByDescending(ex => ex.UpdatedDateTime),
        "asc" => workouts.OrderBy(ex => ex.UpdatedDateTime),
        _ => workouts.OrderByDescending(ex => ex.UpdatedDateTime),
      },
      _ => workouts.OrderByDescending(ex => ex.CreatedDateTime),
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