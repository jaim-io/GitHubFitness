using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Application.Users.Queries.GetSavedMuscleGroupPage;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Users.Queries.GetSavedMusclePage;

public class GetSavedMusclePageQueryHandler
  : IRequestHandler<GetSavedMusclePageQuery, ErrorOr<Pagination<Muscle>>>
{
  private readonly IUserRepository _userRepository;
  private readonly IMuscleRepository _muscleRepository;

  public GetSavedMusclePageQueryHandler(
    IUserRepository userRepository,
    IMuscleRepository muscleRepository)
  {
    _userRepository = userRepository;
    _muscleRepository = muscleRepository;
  }

  public async Task<ErrorOr<Pagination<Muscle>>> Handle(
    GetSavedMusclePageQuery query,
    CancellationToken cancellationToken)
  {
    var userId = UserId.Create(query.UserId);
    if (await _userRepository.GetByIdAsync(userId) is not User user)
    {
      return Errors.User.NotFound;
    }

    var muscleIds = user.SavedMuscleIds.ToList();
    IEnumerable<Muscle> muscles = query.SearchQuery is not null
      ? await _muscleRepository.GetBySearchQueryAndIdsAsync(query.SearchQuery, muscleIds)
      : await _muscleRepository.GetByIdAsync(muscleIds);

    var pageNumber = query.PageNumber ?? 1;

    muscles = query.Sort switch
    {
      "name" => query.Order switch
      {
        "asc" => muscles.OrderBy(ex => ex.Name),
        "desc" => muscles.OrderByDescending(ex => ex.Name),
        _ => muscles.OrderByDescending(ex => ex.Name),
      },
      "created" => query.Order switch
      {
        "desc" => muscles.OrderByDescending(ex => ex.CreatedDateTime),
        "asc" => muscles.OrderBy(ex => ex.CreatedDateTime),
        _ => muscles.OrderByDescending(ex => ex.CreatedDateTime),
      },
      "updated" => query.Order switch
      {
        "desc" => muscles.OrderByDescending(ex => ex.UpdatedDateTime),
        "asc" => muscles.OrderBy(ex => ex.UpdatedDateTime),
        _ => muscles.OrderByDescending(ex => ex.UpdatedDateTime),
      },
      _ => muscles.OrderByDescending(ex => ex.CreatedDateTime),
    };

    decimal pageCount = query.PageSize == null
      ? 1
      : Math.Ceiling((decimal)muscles.Count() / (int)query.PageSize);

    muscles = muscles
      .Skip((pageNumber - 1) * query.PageSize ?? 0)
      .ToList();

    if (!(pageNumber == 1 && pageCount == 0) &&
        pageNumber > pageCount)
    {
      return Errors.Page.NotFound;
    }

    muscles = query.PageSize == null
      ? muscles
      : muscles
        .Take((int)query.PageSize);

    return new Pagination<Muscle>(
      muscles.ToList(),
      pageNumber,
      (int)pageCount);
  }
}