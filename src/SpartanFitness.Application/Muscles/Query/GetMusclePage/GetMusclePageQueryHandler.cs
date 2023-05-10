using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Application.Muscles.Query.GetMusclePage;

public class GetMusclePageQueryHandler : IRequestHandler<GetMusclePageQuery, ErrorOr<Pagination<Muscle>>>
{
  private readonly IMuscleRepository _muscleRepository;

  public GetMusclePageQueryHandler(IMuscleRepository muscleRepository)
  {
    _muscleRepository = muscleRepository;
  }

  public async Task<ErrorOr<Pagination<Muscle>>> Handle(GetMusclePageQuery query, CancellationToken cancellationToken)
  {
    var pageNumber = query.PageNumber ?? 1;

    IEnumerable<Muscle> muscles = query.SearchQuery is not null
      ? await _muscleRepository.GetBySearchQueryAsync(query.SearchQuery)
      : await _muscleRepository.GetAllAsync();

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

    decimal pageCount = query.PageSize == null
      ? 1
      : Math.Ceiling((decimal)muscles.Count() / (int)query.PageSize);

    // .ToList() -> Possible multiple enumeration of IEnumerable
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