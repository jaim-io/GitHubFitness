using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Muscles.Query.GetMusclePageByMuscleGroup;

public class GetMusclePageByMuscleGroupQueryHandler
  : IRequestHandler<GetMusclePageByMuscleGroupQuery, ErrorOr<Page<Muscle>>>
{
  private readonly IMuscleRepository _muscleRepository;
  private readonly IMuscleGroupRepository _muscleGroupRepository;

  public GetMusclePageByMuscleGroupQueryHandler(
    IMuscleRepository muscleRepository,
    IMuscleGroupRepository muscleGroupRepository)
  {
    _muscleRepository = muscleRepository;
    _muscleGroupRepository = muscleGroupRepository;
  }

  public async Task<ErrorOr<Page<Muscle>>> Handle(
    GetMusclePageByMuscleGroupQuery query,
    CancellationToken cancellationToken)
  {
    var muscleGroupId = MuscleGroupId.Create(query.MuscleGroupId);

    if (await _muscleGroupRepository.GetByIdAsync(muscleGroupId) is not null)
    {
      return Errors.MuscleGroup.NotFound;
    }

    var pageNumber = query.PageNumber ?? 1;

    IEnumerable<Muscle> muscles = query.SearchQuery is not null
      ? await _muscleRepository.GetBySearchQueryAsync(query.SearchQuery, muscleGroupId)
      : await _muscleRepository.GetByMuscleGroupIdAsync(muscleGroupId);

    muscles = query.Sort switch
    {
      "name" => query.Order switch
      {
        "asc" => muscles.OrderBy(m => m.Name),
        "desc" => muscles.OrderByDescending(m => m.Name),
        _ => muscles.OrderByDescending(m => m.Name),
      },
      "created" => query.Order switch
      {
        "desc" => muscles.OrderByDescending(m => m.CreatedDateTime),
        "asc" => muscles.OrderBy(m => m.CreatedDateTime),
        _ => muscles.OrderByDescending(m => m.CreatedDateTime),
      },
      "updated" => query.Order switch
      {
        "desc" => muscles.OrderByDescending(m => m.UpdatedDateTime),
        "asc" => muscles.OrderBy(m => m.UpdatedDateTime),
        _ => muscles.OrderByDescending(m => m.UpdatedDateTime),
      },
      _ => muscles.OrderByDescending(m => m.CreatedDateTime),
    };

    // .ToList() -> Possible multiple enumeration of IEnumerable
    muscles = muscles
      .Skip((pageNumber - 1) * query.PageSize ?? 0)
      .ToList();

    decimal pageCount = query.PageSize == null
      ? 1
      : Math.Ceiling((decimal)muscles.Count() / (int)query.PageSize);

    if (!(pageNumber == 1 && pageCount == 0) &&
        pageNumber > pageCount)
    {
      return Errors.Page.NotFound;
    }

    muscles = query.PageSize == null
      ? muscles
      : muscles
        .Take((int)query.PageSize);

    return new Page<Muscle>(
      muscles.ToList(),
      pageNumber,
      (int)pageCount);
  }
}