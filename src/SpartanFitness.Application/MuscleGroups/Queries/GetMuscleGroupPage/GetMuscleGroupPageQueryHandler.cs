using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Application.MuscleGroups.Queries.GetMuscleGroupPage;

public class GetMuscleGroupPageQueryHandler : IRequestHandler<GetMuscleGroupPageQuery, ErrorOr<Pagination<MuscleGroup>>>
{
  private readonly IMuscleGroupRepository _muscleGroupRepository;

  public GetMuscleGroupPageQueryHandler(IMuscleGroupRepository muscleGroupRepository)
  {
    _muscleGroupRepository = muscleGroupRepository;
  }

  public async Task<ErrorOr<Pagination<MuscleGroup>>> Handle(
    GetMuscleGroupPageQuery query,
    CancellationToken cancellationToken)
  {
    var pageNumber = query.PageNumber ?? 1;

    IEnumerable<MuscleGroup> muscleGroups = query.SearchQuery is not null
      ? await _muscleGroupRepository.GetBySearchQueryAsync(query.SearchQuery)
      : await _muscleGroupRepository.GetAllAsync();

    muscleGroups = query.Sort switch
    {
      "name" => query.Order switch
      {
        "asc" => muscleGroups.OrderBy(mg => mg.Name),
        "desc" => muscleGroups.OrderByDescending(mg => mg.Name),
        _ => muscleGroups.OrderByDescending(mg => mg.Name),
      },
      "created" => query.Order switch
      {
        "desc" => muscleGroups.OrderByDescending(mg => mg.CreatedDateTime),
        "asc" => muscleGroups.OrderBy(mg => mg.CreatedDateTime),
        _ => muscleGroups.OrderByDescending(mg => mg.CreatedDateTime),
      },
      "updated" => query.Order switch
      {
        "desc" => muscleGroups.OrderByDescending(mg => mg.UpdatedDateTime),
        "asc" => muscleGroups.OrderBy(mg => mg.UpdatedDateTime),
        _ => muscleGroups.OrderByDescending(mg => mg.UpdatedDateTime),
      },
      _ => muscleGroups.OrderByDescending(mg => mg.CreatedDateTime),
    };

    decimal pageCount = query.PageSize == null
      ? 1
      : Math.Ceiling((decimal)muscleGroups.Count() / (int)query.PageSize);

    // .ToList() -> Possible multiple enumeration of IEnumerable
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