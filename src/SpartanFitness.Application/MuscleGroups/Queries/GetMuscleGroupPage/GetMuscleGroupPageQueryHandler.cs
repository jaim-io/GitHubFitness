using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.MuscleGroups.Queries.GetMuscleGroupPage;

public class GetMuscleGroupPageQueryHandler : IRequestHandler<GetMuscleGroupPageQuery, ErrorOr<Page<MuscleGroup>>>
{
  private readonly IMuscleGroupRepository _muscleGroupRepository;

  public GetMuscleGroupPageQueryHandler(IMuscleGroupRepository muscleGroupRepository)
  {
    _muscleGroupRepository = muscleGroupRepository;
  }

  public async Task<ErrorOr<Page<MuscleGroup>>> Handle(GetMuscleGroupPageQuery query, CancellationToken cancellationToken)
  {
    Func<MuscleGroup, bool>? filter = null;
    if (query.SearchQuery is not null)
    {
      string searchQuery = query.SearchQuery.ToLower();
      filter = (mg) => mg.Name.ToLower().Contains(searchQuery) || mg.Description.ToLower().Contains(searchQuery);
    }

    var pageNumber = query.PageNumber ?? 1;

    IEnumerable<MuscleGroup> skippedMuscleGroups;
    decimal pageCount;
    {
      IEnumerable<MuscleGroup> muscleGroups;
      if (filter != null)
      {
        muscleGroups = _muscleGroupRepository.GetAllWithFilter(filter);
      }
      else
      {
        muscleGroups = await _muscleGroupRepository.GetAllAsync();
      }

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

      skippedMuscleGroups = muscleGroups.Skip((pageNumber - 1) * query.PageSize ?? 0);
      pageCount = query.PageSize == null
        ? 1
        : Math.Ceiling((decimal)muscleGroups.Count() / (int)query.PageSize);
    }

    if (!(pageNumber == 1 && pageCount == 0) &&
      pageNumber > pageCount)
    {
      return Errors.Page.NotFound;
    }

    var content = query.PageSize == null
      ? skippedMuscleGroups
      : skippedMuscleGroups
          .Take((int)query.PageSize);

    return new Page<MuscleGroup>(
      content.ToList(),
      pageNumber,
      (int)pageCount);
  }
}