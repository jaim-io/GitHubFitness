using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SpartanFitness.Application.Muscles.Command.CreateMuscle;
using SpartanFitness.Application.Muscles.Command.UpdateMuscle;
using SpartanFitness.Application.Muscles.Query.GetAllMuscles;
using SpartanFitness.Application.Muscles.Query.GetMuscleById;
using SpartanFitness.Application.Muscles.Query.GetMusclePage;
using SpartanFitness.Application.Muscles.Query.GetMusclesById;
using SpartanFitness.Application.Muscles.Query.GetMusclesByMuscleGroupId;
using SpartanFitness.Contracts.Common;
using SpartanFitness.Contracts.Muscles;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.Enums;

namespace SpartanFitness.Api.Controllers.V1;

[Route("api/v{version:apiVersion}/muscles")]
public class MusclesController : ApiController
{
  private readonly ISender _mediator;
  private readonly IMapper _mapper;

  public MusclesController(ISender mediator, IMapper mapper)
  {
    _mediator = mediator;
    _mapper = mapper;
  }

  [HttpGet("ids/{id?}")]
  [AllowAnonymous]
  public async Task<IActionResult> GetMusclesByIds([FromQuery(Name = "id")] List<string> ids)
  {
    var query = new GetMusclesByIdQuery(ids);
    ErrorOr<List<Muscle>> musclesResult = await _mediator.Send(query);

    return musclesResult.Match(
      muscles => Ok(_mapper.Map<MuscleResponse[]>(muscles)),
      Problem);
  }

  [HttpGet("{muscleId}")]
  [AllowAnonymous]
  public async Task<IActionResult> GetMuscle(string muscleId)
  {
    var query = new GetMuscleByIdQuery(muscleId);
    ErrorOr<Muscle> muscleResult = await _mediator.Send(query);

    return muscleResult.Match(
      muscle => Ok(_mapper.Map<MuscleResponse>(muscle)),
      Problem);
  }

  [HttpGet]
  [AllowAnonymous]
  public async Task<IActionResult> GetAllMuscle()
  {
    var query = new GetAllMusclesQuery();
    ErrorOr<List<Muscle>> musclesResult = await _mediator.Send(query);

    return musclesResult.Match(
      muscles => Ok(_mapper.Map<List<MuscleResponse>>(muscles)),
      Problem);
  }

  [HttpGet("muscle-group-ids/{id?}")]
  [AllowAnonymous]
  public async Task<IActionResult> GetMusclesByMuscleGroupIds([FromQuery(Name = "id")] List<string> ids)
  {
    var query = new GetMusclesByMuscleGroupIdQuery(ids);
    ErrorOr<List<Muscle>> musclesResult = await _mediator.Send(query);

    return musclesResult.Match(
      muscles => Ok(_mapper.Map<MuscleResponse[]>(muscles)),
      Problem);
  }

  [HttpGet("page/{p:int?}/{ls:int?}/{s?}/{o?}/{q?}")]
  [AllowAnonymous]
  public async Task<IActionResult> GetMusclesPage([FromQuery] PagingRequest request)
  {
    var query = _mapper.Map<GetMusclePageQuery>(request);
    ErrorOr<Pagination<Muscle>> musclePageResult = await _mediator.Send(query);

    return musclePageResult.Match(
      musclePage => Ok(_mapper.Map<MusclePageResponse>(musclePage)),
      Problem);
  }

  [HttpPost]
  public async Task<IActionResult> CreateMuscle([FromBody] CreateMuscleRequest request)
  {
    var command = _mapper.Map<CreateMuscleCommand>(request);
    ErrorOr<Muscle> muscleResult = await _mediator.Send(command);

    return muscleResult.Match(
      muscle => CreatedAtAction(
        nameof(GetMuscle),
        new { muscleId = muscle.Id.Value },
        _mapper.Map<MuscleResponse>(muscle)),
      Problem);
  }

  [HttpPut("{muscleId}")]
  [Authorize(Roles = $"{RoleTypes.Administrator}")]
  public async Task<IActionResult> UpdateMuscle(
    [FromBody] UpdateMuscleRequest request,
    [FromRoute] string muscleId)
  {
    if (request.Id != muscleId)
    {
      return BadRequest("Route ID must match the ID field in the request body.");
    }

    var command = _mapper.Map<UpdateMuscleCommand>(request);
    ErrorOr<Muscle> muscleResult = await _mediator.Send(command);

    return muscleResult.Match(
      muscle => Ok(_mapper.Map<MuscleResponse>(muscle)),
      Problem);
  }
}