using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SpartanFitness.Application.MuscleGroups.Commands.CreateMuscleGroup;
using SpartanFitness.Application.MuscleGroups.Commands.UpdateMuscleGroup;
using SpartanFitness.Application.MuscleGroups.Queries.GetAllMuscleGroups;
using SpartanFitness.Application.MuscleGroups.Queries.GetMuscleGroupById;
using SpartanFitness.Application.MuscleGroups.Queries.GetMuscleGroupPage;
using SpartanFitness.Application.MuscleGroups.Queries.GetMuscleGroupsById;
using SpartanFitness.Application.MuscleGroups.Queries.GetMuscleGroupsByMuscleIds;
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

  [HttpGet("page/{p:int?}/{ls:int?}/{s?}/{o?}/{q?}")]
  [AllowAnonymous]
  public async Task<IActionResult> GetMuscleGroupsPage([FromQuery] PagingRequest request)
  {
    var query = _mapper.Map<GetMuscleGroupPageQuery>(request);
    ErrorOr<Pagination<MuscleGroup>> muscleGroupResult = await _mediator.Send(query);

    return muscleGroupResult.Match(
      muscleGroupPage => Ok(_mapper.Map<MuscleGroupPageResponse>(muscleGroupPage)),
      errors => Problem(errors));
  }

  [HttpGet("ids/{id?}")]
  [AllowAnonymous]
  public async Task<IActionResult> GetMuscleGroupsByIds([FromQuery(Name = "id")] List<string> ids)
  {
    var query = new GetMuscleGroupsByIdQuery(ids);
    ErrorOr<List<MuscleGroup>> muscleGroupsResult = await _mediator.Send(query);

    return muscleGroupsResult.Match(
      muscleGroups => Ok(_mapper.Map<MuscleGroupResponse[]>(muscleGroups)),
      Problem);
  }

  [HttpGet]
  [AllowAnonymous]
  public async Task<IActionResult> GetAllMuscleGroups()
  {
    var query = new GetAllMuscleGroupsQuery();
    ErrorOr<List<MuscleGroup>> result = await _mediator.Send(query);

    return result.Match(
      muscleGroups => Ok(_mapper.Map<MuscleGroupResponse[]>(muscleGroups)),
      Problem);
  }

  [HttpGet("muscle-ids/{id?}")]
  [AllowAnonymous]
  public async Task<IActionResult> GetMuscleGroupsByMuscleIds([FromQuery(Name = "id")] List<string> ids)
  {
    var query = new GetMuscleGroupsByMuscleIdsQuery(ids);
    ErrorOr<List<MuscleGroup>> muscleGroupsResult = await _mediator.Send(query);

    return muscleGroupsResult.Match(
      muscleGroups => Ok(_mapper.Map<MuscleGroupResponse[]>(muscleGroups)),
      Problem);
  }

  [HttpGet("{muscleGroupId}")]
  [AllowAnonymous]
  public async Task<IActionResult> GetMuscleGroup(string muscleGroupId)
  {
    var query = new GetMuscleGroupByIdQuery(muscleGroupId);
    ErrorOr<MuscleGroup> muscleGroupResult = await _mediator.Send(query);

    return muscleGroupResult.Match(
      muscleGroup => Ok(_mapper.Map<MuscleGroupResponse>(muscleGroup)),
      errors => Problem(errors));
  }

  [HttpPost]
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

  [HttpPut("{muscleGroupId}")]
  [Authorize(Roles = $"{RoleTypes.Administrator}")]
  public async Task<IActionResult> UpdateMuscleGroup(
  [FromBody] UpdateMuscleGroupRequest request,
  [FromRoute] string muscleGroupId)
  {
    if (request.Id != muscleGroupId)
    {
      return BadRequest("Route ID must match the ID field in the request body.");
    }

    var command = _mapper.Map<UpdateMuscleGroupCommand>(request);
    ErrorOr<MuscleGroup> updatedMuscleGroupResult = await _mediator.Send(command);

    return updatedMuscleGroupResult.Match(
      muscleGroup => Ok(_mapper.Map<MuscleGroupResponse>(muscleGroup)),
      Problem);
  }
}