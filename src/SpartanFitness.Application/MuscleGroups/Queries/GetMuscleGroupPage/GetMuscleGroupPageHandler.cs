using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.MuscleGroups.Queries.GetMuscleGroupPage;

public class GetMuscleGroupPageHandler : IRequestHandler<GetMuscleGroupPageQuery, ErrorOr<Page<MuscleGroup>>>
{
  private readonly IMuscleGroupRepository _muscleGroupRepository;

  public GetMuscleGroupPageHandler(IMuscleGroupRepository muscleGroupRepository)
  {
    _muscleGroupRepository = muscleGroupRepository;
  }

  public async Task<ErrorOr<Page<MuscleGroup>>> Handle(GetMuscleGroupPageQuery query, CancellationToken cancellationToken)
  {
    Func<MuscleGroup, bool>? filter = null;
    if (Guid.TryParse(query.SearchQuery, out Guid guid))
    {
      var muscleGroupId = MuscleGroupId.Create(guid);
      var creatorId = CoachId.Create(guid);

      filter = (mg) => mg.CreatorId.Equals(creatorId);
    }
    else if (query.SearchQuery is not null)
    {
      string searchQuery = query.SearchQuery.ToLower();
      filter = (mg) => mg.Name.ToLower().Contains(searchQuery) || mg.Description.ToLower().Contains(searchQuery);
    }

    var pageNumber = query.PageNumber ?? 1;

    IEnumerable<MuscleGroup> skippedMuscles;
    decimal pageCount;
    {
      IEnumerable<MuscleGroup> muscles;
      if (filter != null)
      {
        muscles = _muscleGroupRepository.GetAllWithFilter(filter);
      }
      else
      {
        muscles = await _muscleGroupRepository.GetAllAsync();
      }

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

    return new Page<MuscleGroup>(
      content.ToList(),
      pageNumber,
      (int)pageCount);
  }
}