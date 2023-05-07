using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using SpartanFitness.Application.Muscles.Command.CreateMuscle;
using SpartanFitness.Application.Muscles.Query.GetMuscleById;
using SpartanFitness.Application.Muscles.Query.GetMusclePage;
using SpartanFitness.Application.Muscles.Query.GetMusclesById;
using SpartanFitness.Contracts.Common;
using SpartanFitness.Contracts.Muscles;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Models;

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

  [HttpGet("{muscleId}")]
  public async Task<IActionResult> GetMuscle([FromQuery] string muscleId)
  {
    var query = new GetMuscleByIdQuery(muscleId);
    ErrorOr<Muscle> muscleResult = await _mediator.Send(query);

    return muscleResult.Match(
      muscle => Ok(_mapper.Map<MuscleResponse>(muscle)),
      Problem);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetMusclesByIds([FromQuery(Name = "id")] List<string> ids)
  {
    var query = new GetMusclesByIdQuery(ids);
    ErrorOr<List<Muscle>> musclesResult = await _mediator.Send(query);

    return musclesResult.Match(
      muscles => Ok(_mapper.Map<MuscleResponse[]>(muscles)),
      Problem);
  }

  [HttpGet("{p:int?}/{ls:int?}/{s?}/{o?}/{q?}")]
  public async Task<IActionResult> GetMuscles([FromQuery] PagingRequest request)
  {
    var query = _mapper.Map<GetMusclePageQuery>(request);
    ErrorOr<Pagination<Muscle>> musclePageResult = await _mediator.Send(query);

    return musclePageResult.Match(
      musclePage => Ok(_mapper.Map<MusclePageResponse>(musclePage)),
      Problem);
  }

  [HttpPost("create")]
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
}