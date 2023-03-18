using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SpartanFitness.Application.Workouts.CreateWorkout;
using SpartanFitness.Contracts.Workouts;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Enums;

namespace SpartanFitness.Api.Controllers;

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
  public IActionResult GetWorkout(string coachId, string workoutId)
  {
    return Ok(workoutId);
  }

  [HttpPost("create")]
  [Authorize(Roles = RoleTypes.Coach)]
  public async Task<IActionResult> CreateWorkout(CreateWorkoutRequest request, string coachId)
  {
    // Verify if user is coach with given CoachId: Func<string, string, bool>(string userId, string coachId) => bool 
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