using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Users.Queries.GetSavedMuscleGroupPage;

public class GetSavedMuscleGroupPageQueryHandler
  : IRequestHandler<GetSavedMuscleGroupPageQuery, ErrorOr<Pagination<MuscleGroup>>>
{
  private readonly IUserRepository _userRepository;
  private readonly IMuscleGroupRepository _muscleGroupRepository;

  public GetSavedMuscleGroupPageQueryHandler(
    IUserRepository userRepository,
    IMuscleGroupRepository muscleGroupRepository)
  {
    _userRepository = userRepository;
    _muscleGroupRepository = muscleGroupRepository;
  }

  public async Task<ErrorOr<Pagination<MuscleGroup>>> Handle(
    GetSavedMuscleGroupPageQuery query,
    CancellationToken cancellationToken)
  {
    var userId = UserId.Create(query.UserId);
    if (await _userRepository.GetByIdAsync(userId) is not User user)
    {
      return Errors.User.NotFound;
    }

    var muscleGroupIds = user.SavedMuscleGroupIds.ToList();
    IEnumerable<MuscleGroup> muscleGroups = query.SearchQuery is not null
      ? await _muscleGroupRepository.GetBySearchQueryAndIdsAsync(query.SearchQuery, muscleGroupIds)
      : await _muscleGroupRepository.GetByIdAsync(muscleGroupIds);

    var pageNumber = query.PageNumber ?? 1;

    muscleGroups = query.Sort switch
    {
      "name" => query.Order switch
      {
        "asc" => muscleGroups.OrderBy(ex => ex.Name),
        "desc" => muscleGroups.OrderByDescending(ex => ex.Name),
        _ => muscleGroups.OrderByDescending(ex => ex.Name),
      },
      "created" => query.Order switch
      {
        "desc" => muscleGroups.OrderByDescending(ex => ex.CreatedDateTime),
        "asc" => muscleGroups.OrderBy(ex => ex.CreatedDateTime),
        _ => muscleGroups.OrderByDescending(ex => ex.CreatedDateTime),
      },
      "updated" => query.Order switch
      {
        "desc" => muscleGroups.OrderByDescending(ex => ex.UpdatedDateTime),
        "asc" => muscleGroups.OrderBy(ex => ex.UpdatedDateTime),
        _ => muscleGroups.OrderByDescending(ex => ex.UpdatedDateTime),
      },
      _ => muscleGroups.OrderByDescending(ex => ex.CreatedDateTime),
    };

    decimal pageCount = query.PageSize == null
      ? 1
      : Math.Ceiling((decimal)muscleGroups.Count() / (int)query.PageSize);

    muscleGroups = muscleGroups
      .Skip((pageNumber - 1) * query.PageSize ?? 0)
      .ToList();

    if (!(pageNumber == 1 && pageCount == 0) &&
        pageNumber > pageCount)
    {
      return Errors.Page.NotFound;
    }

    muscleGroups = query.PageSize == null
      ? muscleGroups
      : muscleGroups
        .Take((int)query.PageSize);

    return new Pagination<MuscleGroup>(
      muscleGroups.ToList(),
      pageNumber,
      (int)pageCount);
  }
}