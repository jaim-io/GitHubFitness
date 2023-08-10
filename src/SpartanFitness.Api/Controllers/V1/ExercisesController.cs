using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SpartanFitness.Application.Exercises.Commands.CreateExercise;
using SpartanFitness.Application.Exercises.Commands.DeleteExercise;
using SpartanFitness.Application.Exercises.Commands.UpdateExercise;
using SpartanFitness.Application.Exercises.Queries.GetAllExercises;
using SpartanFitness.Application.Exercises.Queries.GetExerciseById;
using SpartanFitness.Application.Exercises.Queries.GetExercisePage;
using SpartanFitness.Contracts.Common;
using SpartanFitness.Contracts.Exercises;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.Enums;

namespace SpartanFitness.Api.Controllers.V1;

[Route("api/v{version:apiVersion}/[controller]")]
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

  [HttpGet("page/{p:int?}/{ls:int?}/{s?}/{o?}/{q?}")]
  [AllowAnonymous]
  public async Task<IActionResult> GetExercisesPage([FromQuery] PagingRequest request)
  {
    var query = _mapper.Map<GetExercisePageQuery>(request);
    ErrorOr<Pagination<Exercise>> exercisesResult = await _mediator.Send(query);

    return exercisesResult.Match(
      exercisesPage => Ok(_mapper.Map<ExercisePageResponse>(exercisesPage)),
      Problem);
  }

  [HttpGet("{exerciseId}")]
  [AllowAnonymous]
  public async Task<IActionResult> GetExercise(string exerciseId)
  {
    var query = new GetExerciseByIdQuery(exerciseId);
    ErrorOr<Exercise> exerciseResult = await _mediator.Send(query);

    return exerciseResult.Match(
      exercise => Ok(_mapper.Map<ExerciseResponse>(exercise)),
      Problem);
  }

  [HttpGet]
  [AllowAnonymous]
  public async Task<IActionResult> GetAllExercises()
  {
    var query = new GetAllExercisesQuery();
    ErrorOr<List<Exercise>> exerciseResult = await _mediator.Send(query);

    return exerciseResult.Match(
      exercises => Ok(_mapper.Map<List<ExerciseResponse>>(exercises)),
      Problem);
  }

  [HttpPost]
  [Authorize(Roles = $"{RoleTypes.Coach}, {RoleTypes.Administrator}")]
  public async Task<IActionResult> CreateExercise(CreateExerciseRequest request)
  {
    var userId = Authorization.GetUserId(HttpContext);
    var command = _mapper.Map<CreateExerciseCommand>((request, userId));
    ErrorOr<Exercise> createdExerciseResult = await _mediator.Send(command);

    return createdExerciseResult.Match(
      exercise => CreatedAtAction(
        nameof(GetExercise),
        new { exerciseId = exercise.Id.Value },
        _mapper.Map<ExerciseResponse>(exercise)),
      Problem);
  }

  [HttpPut("{exerciseId}")]
  [Authorize(Roles = $"{RoleTypes.Coach}, {RoleTypes.Administrator}")]
  public async Task<IActionResult> UpdateExercise(
    [FromBody] UpdateExerciseRequest request,
    [FromRoute] string exerciseId)
  {
    if (request.Id != exerciseId)
    {
      return BadRequest("Route ID must match the ID field in the request body.");
    }

    var coachId = Authorization.GetCoachId(HttpContext);
    var command = _mapper.Map<UpdateExerciseCommand>((request, coachId)) with { Image = request.Image, };
    ErrorOr<Exercise> updatedExerciseResult = await _mediator.Send(command);

    return updatedExerciseResult.Match(
      exercise => Ok(_mapper.Map<ExerciseResponse>(exercise)),
      Problem);
  }

  [HttpDelete("{exerciseId}")]
  [Authorize(Roles = $"{RoleTypes.Coach}, {RoleTypes.Administrator}")]
  public async Task<IActionResult> DeleteExercise([FromRoute] string exerciseId)
  {
    var coachId = Authorization.GetCoachId(HttpContext);
    var adminId = Authorization.GetAdminId(HttpContext);

    var command = new DeleteExerciseCommand(
      CoachId: coachId,
      AdminId: adminId,
      ExerciseId: exerciseId);
    ErrorOr<Unit> result = await _mediator.Send(command);

    return result.Match(
      (_) => NoContent(),
      Problem);
  }
}