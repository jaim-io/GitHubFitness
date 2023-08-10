﻿using ErrorOr;

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
using SpartanFitness.Application.Users.Commands.UnSaveMuscle;
using SpartanFitness.Application.Users.Commands.UnSaveMuscleGroup;
using SpartanFitness.Application.Users.Commands.UnSaveWorkout;
using SpartanFitness.Application.Users.Queries.GetAllSavedExerciseIds;
using SpartanFitness.Application.Users.Queries.GetAllSavedMuscleGroupIds;
using SpartanFitness.Application.Users.Queries.GetAllSavedMuscleIds;
using SpartanFitness.Application.Users.Queries.GetAllSavedWorkoutIds;
using SpartanFitness.Application.Users.Queries.GetUserSaves;
using SpartanFitness.Contracts.Users;
using SpartanFitness.Contracts.Users.Saves;
using SpartanFitness.Contracts.Users.Saves.Requests;
using SpartanFitness.Contracts.Users.Saves.Responses;
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

  [HttpPatch("exercises/add")]
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

  [HttpPatch("exercises/remove")]
  public async Task<IActionResult> UnSaveExercise([FromRoute] string userId, [FromBody] UnSaveExerciseRequest request)
  {
    var isUser = Authorization.UserIdMatchesClaim(HttpContext, userId);
    if (!isUser)
    {
      return Unauthorized();
    }

    var command = _mapper.Map<UnSaveExerciseCommand>((request, userId));
    ErrorOr<Unit> result = await _mediator.Send(command);

    return result.Match(
      _ => NoContent(),
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

  [HttpPatch("muscle-groups/add")]
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

  [HttpPatch("muscle-groups/remove")]
  public async Task<IActionResult> UnSaveMuscleGroup(
    [FromRoute] string userId,
    [FromBody] UnSaveMuscleGroupRequest request)
  {
    var isUser = Authorization.UserIdMatchesClaim(HttpContext, userId);
    if (!isUser)
    {
      return Unauthorized();
    }

    var command = _mapper.Map<UnSaveMuscleGroupCommand>((request, userId));
    ErrorOr<Unit> result = await _mediator.Send(command);

    return result.Match(
      _ => NoContent(),
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
  
  [HttpPatch("muscles/add")]
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

  [HttpPatch("muscles/remove")]
  public async Task<IActionResult> UnSaveMuscle([FromRoute] string userId, [FromBody] UnSaveMuscleRequest request)
  {
    var isUser = Authorization.UserIdMatchesClaim(HttpContext, userId);
    if (!isUser)
    {
      return Unauthorized();
    }

    var command = _mapper.Map<UnSaveMuscleCommand>((request, userId));
    ErrorOr<Unit> result = await _mediator.Send(command);

    return result.Match(
      _ => NoContent(),
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

  [HttpPatch("workouts/add")]
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

  [HttpPatch("workouts/remove")]
  public async Task<IActionResult> UnSaveWorkout([FromRoute] string userId, [FromBody] UnSaveWorkoutRequest request)
  {
    var isUser = Authorization.UserIdMatchesClaim(HttpContext, userId);
    if (!isUser)
    {
      return Unauthorized();
    }

    var command = _mapper.Map<UnSaveWorkoutCommand>((request, userId));
    ErrorOr<Unit> result = await _mediator.Send(command);

    return result.Match(
      _ => NoContent(),
      Problem);
  }
}