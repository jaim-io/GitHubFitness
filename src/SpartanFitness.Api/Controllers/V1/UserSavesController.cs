using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using SpartanFitness.Application.Users.Commands.SaveExercise;
using SpartanFitness.Application.Users.Commands.SaveMuscleGroup;
using SpartanFitness.Application.Users.Commands.UnSaveExercise;
using SpartanFitness.Application.Users.Commands.UnSaveMuscleGroup;
using SpartanFitness.Contracts.Users.Saves;

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
  public async Task<IActionResult> UnSaveMuscleGroup([FromRoute] string userId, [FromBody] UnSaveMuscleGroupRequest request)
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
}