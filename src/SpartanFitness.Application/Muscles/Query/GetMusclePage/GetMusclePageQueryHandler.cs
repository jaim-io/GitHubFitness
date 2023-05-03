using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Muscles.Query.GetMusclePage;

public class GetMusclePageQueryHandler : IRequestHandler<GetMusclePageQuery, ErrorOr<Page<Muscle>>>
{
  private readonly IMuscleRepository _muscleRepository;

  public GetMusclePageQueryHandler(IMuscleRepository muscleRepository)
  {
    _muscleRepository = muscleRepository;
  }

  public async Task<ErrorOr<Page<Muscle>>> Handle(GetMusclePageQuery query, CancellationToken cancellationToken)
  {
    Func<Muscle, bool>? filter = null;
    if (Guid.TryParse(query.SearchQuery, out Guid guid))
    {
      var muscleGroupId = MuscleGroupId.Create(guid);

      filter = (m) => m.MuscleGroupId.Equals(muscleGroupId);
    }
    else if (query.SearchQuery is not null)
    {
      string searchQuery = query.SearchQuery.ToLower();
      filter = (ex) => ex.Name.ToLower().Contains(searchQuery) || ex.Description.ToLower().Contains(searchQuery);
    }

    var pageNumber = query.PageNumber ?? 1;

    IEnumerable<Muscle> skippedMuscles;
    decimal pageCount;
    {
      IEnumerable<Muscle> muscles;
      if (filter != null)
      {
        muscles = _muscleRepository.GetAllWithFilter(filter);
      }
      else
      {
        muscles = await _muscleRepository.GetAllAsync();
      }

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

      skippedMuscles = muscles.Skip((pageNumber - 1) * query.PageSize ?? 0);
      pageCount = query.PageSize == null
        ? 1
        : Math.Ceiling((decimal)muscles.Count() / (int)query.PageSize);
    }

    if (!(pageNumber == 1 && pageCount == 0) &&
      pageNumber > pageCount)
    {
      return Errors.Page.NotFound;
    }

    var content = query.PageSize == null
      ? skippedMuscles
      : skippedMuscles
          .Take((int)query.PageSize);

    return new Page<Muscle>(
      content.ToList(),
      pageNumber,
      (int)pageCount);
  }
}