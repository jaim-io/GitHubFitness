using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SpartanFitness.Application.Coaches.Commands.CreateCoach;
using SpartanFitness.Application.Coaches.Commands.UpdateCoach;
using SpartanFitness.Application.Coaches.Common;
using SpartanFitness.Application.Coaches.Queries.GetCoachById;
using SpartanFitness.Contracts.Coaches;
using SpartanFitness.Domain.Enums;

namespace SpartanFitness.Api.Controllers.V1;

[Route("api/v{version:apiVersion}/[controller]")]
public class CoachesController : ApiController
{
  private readonly ISender _mediator;
  private readonly IMapper _mapper;

  public CoachesController(
    ISender mediator,
    IMapper mapper)
  {
    _mediator = mediator;
    _mapper = mapper;
  }

  [HttpGet("{coachId}")]
  [AllowAnonymous]
  public async Task<IActionResult> GetCoach(string coachId)
  {
    var query = new GetCoachByIdQuery(coachId);
    ErrorOr<CoachResult> coachResult = await _mediator.Send(query);

    return coachResult.Match(
      result => Ok(_mapper.Map<CoachResponse>(result)),
      Problem);
  }

  [HttpPost]
  [Authorize(Roles = RoleTypes.Administrator)]
  public async Task<IActionResult> CreateCoach(CreateCoachRequest request)
  {
    var command = _mapper.Map<CreateCoachCommand>(request);
    ErrorOr<CoachResult> createdCoachResult = await _mediator.Send(command);

    return createdCoachResult.Match(
      result => CreatedAtAction(
        nameof(GetCoach),
        new { coachId = result.Coach.Id.Value },
        _mapper.Map<CoachResponse>(result)),
      Problem);
  }

  [HttpPatch("{coachId}")]
  public async Task<IActionResult> UpdateCoach(string coachId, UpdateCoachRequest request)
  {
    var isVerifiedCoach = Authorization.CoachIdMatchesClaim(HttpContext, coachId);
    var isAdmin = Authorization.IsAdmin(HttpContext);
    if (!(isVerifiedCoach || isAdmin))
    {
      return Unauthorized();
    }

    if (coachId != request.CoachId)
    {
      return BadRequest("Route coachId & request coachId do not match.");
    }

    var command = _mapper.Map<UpdateCoachCommand>(request);
    ErrorOr<CoachResult> result = await _mediator.Send(command);

    return result.Match(
      coachResult => Ok(_mapper.Map<CoachResponse>(coachResult)),
      Problem);
  }
}