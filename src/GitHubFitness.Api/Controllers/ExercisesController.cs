using ErrorOr;

using GitHubFitness.Application.Exercises.CreateExercise;
using GitHubFitness.Application.Exercises.Queries.GetExerciseById;
using GitHubFitness.Application.Exercises.Queries.GetExerciseList;
using GitHubFitness.Contracts.Exercises;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.Enums;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubFitness.Api.Controllers;

[Route("api/v1/[controller]")]
public class ExercisesController : ApiController
{
  private readonly ISender _mediator;
  private readonly IMapper _mapper;

  public ExercisesController(
      ISender mediator,
      IMapper mapper)
  {
    _mediator = mediator;
    _mapper = mapper;
  }

  [HttpGet]
  public async Task<IActionResult> GetExercises()
  {
    var query = new GetExerciseListQuery();
    ErrorOr<List<Exercise>> exercisesResult = await _mediator.Send(query);

    return exercisesResult.Match(
      exercises => Ok(exercises.ConvertAll(e => _mapper.Map<ExerciseResponse>(e))),
      errors => Problem(errors));
  }

  [HttpGet("{exerciseId}")]
  public async Task<IActionResult> GetExercise(string exerciseId)
  {
    var query = new GetExerciseByIdQuery(exerciseId);
    ErrorOr<Exercise> exerciseResult = await _mediator.Send(query);

    return exerciseResult.Match(
      exercise => Ok(_mapper.Map<ExerciseResponse>(exercise)),
      errors => Problem(errors));
  }

  [HttpPost("create")]
  [Authorize(Roles = $"{RoleTypes.Coach}, {RoleTypes.Administrator}")]
  public async Task<IActionResult> CreateExercise(CreateExerciseRequest request)
  {
    var userId = Authorization.GetUserIdFromClaims(HttpContext);
    var command = _mapper.Map<CreateExerciseCommand>((request, userId));
    ErrorOr<Exercise> createdExerciseResult = await _mediator.Send(command);

    return createdExerciseResult.Match(
      exercise => CreatedAtAction(
        nameof(GetExercise),
        new { exerciseId = exercise.Id },
        _mapper.Map<ExerciseResponse>(exercise)),
      errors => Problem(errors));
  }
}