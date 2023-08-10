using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SpartanFitness.Application.Common.Results;
using SpartanFitness.Application.Users.Commands.SaveExercise;
using SpartanFitness.Application.Users.Commands.SaveMuscle;
using SpartanFitness.Application.Users.Commands.SaveMuscleGroup;
using SpartanFitness.Application.Users.Commands.SaveWorkout;
using SpartanFitness.Application.Users.Commands.UnSaveExercise;
using SpartanFitness.Application.Users.Commands.UnSaveExerciseRange;
using SpartanFitness.Application.Users.Commands.UnSaveMuscle;
using SpartanFitness.Application.Users.Commands.UnSaveMuscleGroup;
using SpartanFitness.Application.Users.Commands.UnSaveMuscleGroupRange;
using SpartanFitness.Application.Users.Commands.UnSaveMuscleRange;
using SpartanFitness.Application.Users.Commands.UnSaveWorkout;
using SpartanFitness.Application.Users.Commands.UnSaveWorkoutRange;
using SpartanFitness.Application.Users.Queries.GetAllSavedExerciseIds;
using SpartanFitness.Application.Users.Queries.GetAllSavedMuscleGroupIds;
using SpartanFitness.Application.Users.Queries.GetAllSavedMuscleIds;
using SpartanFitness.Application.Users.Queries.GetAllSavedWorkoutIds;
using SpartanFitness.Application.Users.Queries.GetSavedExercisePage;
using SpartanFitness.Application.Users.Queries.GetUserSaves;
using SpartanFitness.Contracts.Common;
using SpartanFitness.Contracts.Exercises;
using SpartanFitness.Contracts.Users;
using SpartanFitness.Contracts.Users.Saves;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.Enums;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Api.Controllers.V1;

[Route("api/v{version:apiVersion}/users/{userId}/saved")]
public class UserSavesController : ApiController
{
  private readonly ISender _mediator;
  private readonly IMapper _mapper;

  public UserSavesController(ISender mediator, IMapper mapper)
  {
    _mediator = mediator;
    _mapper = mapper;
  }

  [HttpGet("all")]
  [Authorize(Roles = $"{RoleTypes.User}, {RoleTypes.Administrator}")]
  public async Task<IActionResult> GetAllSaves([FromRoute] string userId)
  {
    var isUser = Authorization.UserIdMatchesClaim(HttpContext, userId);
    var isAdmin = Authorization.IsAdmin(HttpContext);
    if (!(isUser || isAdmin))
    {
      return Unauthorized();
    }

    var query = new GetUserSavesQuery(userId);
    ErrorOr<UserSavesResult> result = await _mediator.Send(query);

    return result.Match(
      saves => Ok(_mapper.Map<UserSavesResponse>(saves)),
      Problem);
  }

  [HttpGet("exercises/all/ids")]
  public async Task<IActionResult> GetAllSavedExerciseIds([FromRoute] string userId)
  {
    var isUser = Authorization.UserIdMatchesClaim(HttpContext, userId);
    if (!isUser)
    {
      return Unauthorized();
    }

    var query = new GetAllSavedExerciseIdsQuery(userId);
    ErrorOr<List<ExerciseId>> result = await _mediator.Send(query);

    return result.Match(
      ids => Ok(_mapper.Map<SavedExerciseIdsResponse>(ids)),
      Problem);
  }

  [HttpGet("exercises/page/{p:int?}/{ls:int?}/{s?}/{o?}/{q?}")]
  public async Task<IActionResult> GetSavedExercisePage([FromRoute] string userId, [FromQuery] PagingRequest request)
  {
    var query = _mapper.Map<GetSavedExercisePageQuery>((request, userId));
    ErrorOr<Pagination<Exercise>> result = await _mediator.Send(query);

    return result.Match(
      page => Ok(_mapper.Map<ExercisePageResponse>(page)),
      Problem);
  }

  [HttpPatch("exercises")]
  public async Task<IActionResult> SaveExercise([FromRoute] string userId, [FromBody] SaveExerciseRequest request)
  {
    var isUser = Authorization.UserIdMatchesClaim(HttpContext, userId);
    if (!isUser)
    {
      return Unauthorized();
    }

    var command = _mapper.Map<SaveExerciseCommand>((request, userId));
    ErrorOr<Unit> result = await _mediator.Send(command);

    return result.Match(
      _ => NoContent(),
      Problem);
  }

  [HttpDelete("exercises/{exerciseId}")]
  public async Task<IActionResult> UnSaveExercise([FromRoute] string userId, [FromRoute] string exerciseId)
  {
    var isUser = Authorization.UserIdMatchesClaim(HttpContext, userId);
    if (!isUser)
    {
      return Unauthorized();
    }

    var command = new UnSaveExerciseCommand(ExerciseId: exerciseId, UserId: userId);
    ErrorOr<Unit> result = await _mediator.Send(command);

    return result.Match(
      _ => NoContent(),
      Problem);
  }

  [HttpDelete("exercises/ids/{id?}")]
  public async Task<IActionResult> UnSaveExercises(
    [FromRoute] string userId,
    [FromQuery(Name = "id")] List<string> exerciseIds)
  {
    var isUser = Authorization.UserIdMatchesClaim(HttpContext, userId);
    if (!isUser)
    {
      return Unauthorized();
    }

    var command = new UnSaveExerciseRangeCommand(UserId: userId, ExerciseIds: exerciseIds);
    ErrorOr<Unit> result = await _mediator.Send(command);

    return result.Match(
      _ => Ok(),
      Problem);
  }

  [HttpGet("muscle-groups/all/ids")]
  public async Task<IActionResult> GetAllSavedMuscleGroupIds([FromRoute] string userId)
  {
    var isUser = Authorization.UserIdMatchesClaim(HttpContext, userId);
    if (!isUser)
    {
      return Unauthorized();
    }

    var query = new GetAllSavedMuscleGroupIdsQuery(userId);
    ErrorOr<List<MuscleGroupId>> result = await _mediator.Send(query);

    return result.Match(
      ids => Ok(_mapper.Map<SavedMuscleGroupIdsResponse>(ids)),
      Problem);
  }

  [HttpPatch("muscle-groups")]
  public async Task<IActionResult> SaveMuscleGroup([FromRoute] string userId, [FromBody] SaveMuscleGroupRequest request)
  {
    var isUser = Authorization.UserIdMatchesClaim(HttpContext, userId);
    if (!isUser)
    {
      return Unauthorized();
    }

    var command = _mapper.Map<SaveMuscleGroupCommand>((request, userId));
    ErrorOr<Unit> result = await _mediator.Send(command);

    return result.Match(
      _ => NoContent(),
      Problem);
  }

  [HttpDelete("muscle-groups/{muscleGroupId}")]
  public async Task<IActionResult> UnSaveMuscleGroup(
    [FromRoute] string userId,
    [FromRoute] string muscleGroupId)
  {
    var isUser = Authorization.UserIdMatchesClaim(HttpContext, userId);
    if (!isUser)
    {
      return Unauthorized();
    }

    var command = new UnSaveMuscleGroupCommand(MuscleGroupId: muscleGroupId, UserId: userId);
    ErrorOr<Unit> result = await _mediator.Send(command);

    return result.Match(
      _ => NoContent(),
      Problem);
  }

  [HttpDelete("muscle-groups/ids/{id?}")]
  public async Task<IActionResult> UnSaveMuscleGroups(
    [FromRoute] string userId,
    [FromQuery(Name = "id")] List<string> muscleGroupIds)
  {
    var isUser = Authorization.UserIdMatchesClaim(HttpContext, userId);
    if (!isUser)
    {
      return Unauthorized();
    }

    var command = new UnSaveMuscleGroupRangeCommand(UserId: userId, MuscleGroupIds: muscleGroupIds);
    ErrorOr<Unit> result = await _mediator.Send(command);

    return result.Match(
      _ => Ok(),
      Problem);
  }

  [HttpGet("muscles/all/ids")]
  public async Task<IActionResult> GetAllSavedMuscleIds([FromRoute] string userId)
  {
    var isUser = Authorization.UserIdMatchesClaim(HttpContext, userId);
    if (!isUser)
    {
      return Unauthorized();
    }

    var query = new GetAllSavedMuscleIdsQuery(userId);
    ErrorOr<List<MuscleId>> result = await _mediator.Send(query);

    return result.Match(
      ids => Ok(_mapper.Map<SavedMuscleIdsResponse>(ids)),
      Problem);
  }

  [HttpPatch("muscles")]
  public async Task<IActionResult> SaveMuscle([FromRoute] string userId, [FromBody] SaveMuscleRequest request)
  {
    var isUser = Authorization.UserIdMatchesClaim(HttpContext, userId);
    if (!isUser)
    {
      return Unauthorized();
    }

    var command = _mapper.Map<SaveMuscleCommand>((request, userId));
    ErrorOr<Unit> result = await _mediator.Send(command);

    return result.Match(
      _ => NoContent(),
      Problem);
  }

  [HttpDelete("muscles/{muscleId}")]
  public async Task<IActionResult> UnSaveMuscle([FromRoute] string userId, [FromRoute] string muscleId)
  {
    var isUser = Authorization.UserIdMatchesClaim(HttpContext, userId);
    if (!isUser)
    {
      return Unauthorized();
    }

    var command = new UnSaveMuscleCommand(MuscleId: muscleId, UserId: userId);
    ErrorOr<Unit> result = await _mediator.Send(command);

    return result.Match(
      _ => NoContent(),
      Problem);
  }

  [HttpDelete("muscles/ids/{id?}")]
  public async Task<IActionResult> UnSaveMuscles(
    [FromRoute] string userId,
    [FromQuery(Name = "id")] List<string> muscleIds)
  {
    var isUser = Authorization.UserIdMatchesClaim(HttpContext, userId);
    if (!isUser)
    {
      return Unauthorized();
    }

    var command = new UnSaveMuscleRangeCommand(UserId: userId, MuscleIds: muscleIds);
    ErrorOr<Unit> result = await _mediator.Send(command);

    return result.Match(
      _ => Ok(),
      Problem);
  }

  [HttpGet("workouts/all/ids")]
  public async Task<IActionResult> GetAllSavedWorkoutIds([FromRoute] string userId)
  {
    var isUser = Authorization.UserIdMatchesClaim(HttpContext, userId);
    if (!isUser)
    {
      return Unauthorized();
    }

    var query = new GetAllSavedWorkoutIdsQuery(userId);
    ErrorOr<List<WorkoutId>> result = await _mediator.Send(query);

    return result.Match(
      ids => Ok(_mapper.Map<SavedWorkoutIdsResponse>(ids)),
      Problem);
  }

  [HttpPatch("workouts")]
  public async Task<IActionResult> SaveWorkout([FromRoute] string userId, [FromBody] SaveWorkoutRequest request)
  {
    var isUser = Authorization.UserIdMatchesClaim(HttpContext, userId);
    if (!isUser)
    {
      return Unauthorized();
    }

    var command = _mapper.Map<SaveWorkoutCommand>((request, userId));
    ErrorOr<Unit> result = await _mediator.Send(command);

    return result.Match(
      _ => NoContent(),
      Problem);
  }

  [HttpDelete("workouts/{workoutId}")]
  public async Task<IActionResult> UnSaveWorkout([FromRoute] string userId, [FromRoute] string workoutId)
  {
    var isUser = Authorization.UserIdMatchesClaim(HttpContext, userId);
    if (!isUser)
    {
      return Unauthorized();
    }

    var command = new UnSaveWorkoutCommand(WorkoutId: workoutId, UserId: userId);
    ErrorOr<Unit> result = await _mediator.Send(command);

    return result.Match(
      _ => NoContent(),
      Problem);
  }

  [HttpDelete("workouts/ids/{id?}")]
  public async Task<IActionResult> UnSaveWorkouts(
    [FromRoute] string userId,
    [FromQuery(Name = "id")] List<string> workoutIds)
  {
    var isUser = Authorization.UserIdMatchesClaim(HttpContext, userId);
    if (!isUser)
    {
      return Unauthorized();
    }

    var command = new UnSaveWorkoutRangeCommand(UserId: userId, WorkoutIds: workoutIds);
    ErrorOr<Unit> result = await _mediator.Send(command);

    return result.Match(
      _ => Ok(),
      Problem);
  }
}