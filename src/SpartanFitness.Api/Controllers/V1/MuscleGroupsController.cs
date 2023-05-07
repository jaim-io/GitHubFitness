using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SpartanFitness.Application.MuscleGroups.Commands.CreateMuscleGroup;
using SpartanFitness.Application.MuscleGroups.Queries.GetMuscleGroupById;
using SpartanFitness.Application.MuscleGroups.Queries.GetMuscleGroupPage;
using SpartanFitness.Application.MuscleGroups.Queries.GetMuscleGroupsById;
using SpartanFitness.Contracts.Common;
using SpartanFitness.Contracts.MuscleGroups;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.Enums;

namespace SpartanFitness.Api.Controllers.V1;

[Route("api/v{version:apiVersion}/muscle-groups")]
public class MuscleGroupsController : ApiController
{
  private readonly ISender _mediator;
  private readonly IMapper _mapper;

  public MuscleGroupsController(
    ISender mediator,
    IMapper mapper)
  {
    _mediator = mediator;
    _mapper = mapper;
  }

  [HttpGet("{p:int?}/{ls:int?}/{s?}/{o?}/{q?}")]
  public async Task<IActionResult> GetMuscleGroups([FromQuery] PagingRequest request)
  {
    var query = _mapper.Map<GetMuscleGroupPageQuery>(request);
    ErrorOr<Pagination<MuscleGroup>> muscleGroupResult = await _mediator.Send(query);

    return muscleGroupResult.Match(
      muscleGroupPage => Ok(_mapper.Map<MuscleGroupPageResponse>(muscleGroupPage)),
      errors => Problem(errors));
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetMuscleGroupsByIds([FromQuery(Name = "id")] List<string> ids)
  {
    var query = new GetMuscleGroupsByIdQuery(ids);
    ErrorOr<List<MuscleGroup>> muscleGroupsResult = await _mediator.Send(query);

    return muscleGroupsResult.Match(
      muscleGroups => Ok(_mapper.Map<MuscleGroupResponse[]>(muscleGroups)),
      Problem);
  }

  [HttpGet("{muscleGroupId}")]
  public async Task<IActionResult> GetMuscleGroup(string muscleGroupId)
  {
    var query = new GetMuscleGroupByIdQuery(muscleGroupId);
    ErrorOr<MuscleGroup> muscleGroupResult = await _mediator.Send(query);

    return muscleGroupResult.Match(
      muscleGroup => Ok(_mapper.Map<MuscleGroupResponse>(muscleGroup)),
      errors => Problem(errors));
  }

  [HttpPost("create")]
  [Authorize(Roles = RoleTypes.Administrator)]
  public async Task<IActionResult> CreateMuscleGroup(
    CreateMuscleGroupRequest request)
  {
    var command = _mapper.Map<CreateMuscleGroupCommand>(request);
    ErrorOr<MuscleGroup> createdMuscleGroupResult = await _mediator.Send(command);

    return createdMuscleGroupResult.Match(
      muscleGroup => CreatedAtAction(
        nameof(GetMuscleGroup),
        new { muscleGroupId = muscleGroup.Id.Value },
        _mapper.Map<MuscleGroupResponse>(muscleGroup)),
      errors => Problem(errors));
  }
}