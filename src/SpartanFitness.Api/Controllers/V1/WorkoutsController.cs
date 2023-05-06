using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SpartanFitness.Application.Authentication.Queries.VerifyIfUserIsCoach;
using SpartanFitness.Application.Workouts.Commands.CreateWorkout;
using SpartanFitness.Application.Workouts.Queries.GetWorkoutById;
using SpartanFitness.Contracts.Workouts;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Enums;

namespace SpartanFitness.Api.Controllers.V1;

[Route("api/v{version:apiVersion}/coaches/{coachId}/[controller]")]
public class WorkoutsController : ApiController
{
  private readonly IMapper _mapper;
  private readonly ISender _mediator;

  public WorkoutsController(
    IMapper mapper,
    ISender mediator)
  {
    _mapper = mapper;
    _mediator = mediator;
  }

  [HttpGet("{workoutId}")]
  public async Task<IActionResult> GetWorkout(string coachId, string workoutId)
  {
    var query = new GetWorkoutByIdQuery(coachId, workoutId);
    ErrorOr<Workout> workoutResult = await _mediator.Send(query);

    return workoutResult.Match(
      workout => Ok(_mapper.Map<WorkoutResponse>(workout)),
      errors => Problem(errors));
  }

  [HttpPost("create")]
  [Authorize(Roles = RoleTypes.Coach)]
  public async Task<IActionResult> CreateWorkout(CreateWorkoutRequest request, string coachId)
  {
    var query = new VerifyIfUserIsCoachQuery(Authorization.GetUserIdFromClaims(HttpContext), coachId);
    ErrorOr<Coach> isAuthorizedResult = await _mediator.Send(query);
    if (isAuthorizedResult.IsError)
    {
      return Problem(isAuthorizedResult.Errors);
    }

    var command = _mapper.Map<CreateWorkoutCommand>((request, coachId));
    ErrorOr<Workout> workoutResult = await _mediator.Send(command);

    return workoutResult.Match(
      workout => CreatedAtAction(
        nameof(GetWorkout),
        new { coachId = workout.CoachId.Value, workoutId = workout.Id.Value },
        _mapper.Map<WorkoutResponse>(workout)),
      errors => Problem(errors));
  }
}