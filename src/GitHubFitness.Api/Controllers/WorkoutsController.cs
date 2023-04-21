using ErrorOr;

using GitHubFitness.Application.Authentication.Queries.VerifyIfUserIsCoach;
using GitHubFitness.Application.Workouts.Commands.CreateWorkout;
using GitHubFitness.Application.Workouts.Queries.GetWorkoutById;
using GitHubFitness.Contracts.Workouts;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.Enums;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubFitness.Api.Controllers;

[Route("api/v1/coaches/{coachId}/[controller]")]
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
    ErrorOr<Workout> workout = await _mediator.Send(command);

    return workout.Match(
      workout => CreatedAtAction(
        nameof(GetWorkout),
        new { coachId = workout.CoachId, workoutId = workout.Id },
        _mapper.Map<WorkoutResponse>(workout)),
      errors => Problem(errors));
  }
}