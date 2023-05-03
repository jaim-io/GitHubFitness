using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using SpartanFitness.Application.Muscles.Command.CreateMuscle;
using SpartanFitness.Application.Muscles.Query.GetMuscleById;
using SpartanFitness.Contracts.Muscles;
using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Api.Controllers;

[Route("api/v1/muscles")]
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

  [HttpPost("create")]
  public async Task<IActionResult> CreateMuscle([FromBody] CreateMuscleRequest request)
  {
    var command = _mapper.Map<CreateMuscleCommand>(request);
    ErrorOr<Muscle> muscleResult = await _mediator.Send(command);

    return muscleResult.Match(
      muscle => CreatedAtAction(
        nameof(GetMuscle),
        _mapper.Map<MuscleResponse>(muscle),
        new { muscleId = muscle.Id.Value }),
      Problem);
  }
}