using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Application.Exercises.Queries.GetExercisePage;

public class GetExercisePageQueryHandler
  : IRequestHandler<GetExercisePageQuery, ErrorOr<Page<Exercise>>>
{
  private readonly IExerciseRepository _exerciseRepository;

  public GetExercisePageQueryHandler(IExerciseRepository exerciseRepository)
  {
    _exerciseRepository = exerciseRepository;
  }

  public async Task<ErrorOr<Page<Exercise>>> Handle(
    GetExercisePageQuery query,
    CancellationToken cancellationToken)
  {
    // TODO: SearchQuery, Filter, Sort

    await Task.CompletedTask;

    var pageNumber = query.PageNumber ?? 1;

    IEnumerable<Exercise> skippedExercises;
    decimal pageCount;
    {
      var exercises = _exerciseRepository.GetAll();

      skippedExercises = exercises.Skip((pageNumber - 1) * query.PageSize ?? 0);
      pageCount = query.PageSize == null
        ? 1
        : Math.Ceiling((decimal)exercises.Count() / (int)query.PageSize);
    }

    if (pageNumber > pageCount)
    {
      return Errors.Page.NotFound;
    }

    var content = query.PageSize == null
      ? skippedExercises
          .ToList()
      : skippedExercises
          .Take((int)query.PageSize)
          .ToList();

    return new Page<Exercise>(
      content,
      pageNumber,
      (int)pageCount);
  }
}