using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SpartanFitness.Application.MuscleGroups.CreateMuscleGroup;
using SpartanFitness.Application.MuscleGroups.Queries.GetMuscleGroupById;
using SpartanFitness.Contracts.MuscleGroups;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Enums;

namespace SpartanFitness.Api.Controllers;

[Route("api/v1/muscle-groups")]
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

    [HttpGet("{muscleGroupId}")]
    public async Task<IActionResult> GetMuscleGroup(string muscleGroupId)
    {
        var query = new GetMuscleGroupByIdQuery(muscleGroupId);
        ErrorOr<MuscleGroup> muscleGroupResult = await _mediator.Send(query);

        return muscleGroupResult.Match(
            muscleGroupResult => Ok(_mapper.Map<MuscleGroupResponse>(muscleGroupResult)),
            errors => Problem(errors));
    }

    [HttpPost("create")]
    [Authorize(Roles = $"{RoleTypes.Coach}, {RoleTypes.Administrator}")]
    public async Task<IActionResult> CreateMuscleGroup(
      CreateMuscleGroupRequest request)
    {
        var userId = Authorization.GetUserIdFromClaims(HttpContext);
        var command = _mapper.Map<CreateMuscleGroupCommand>((request, userId));
        ErrorOr<MuscleGroup> createdMuscleGroupResult = await _mediator.Send(command);

        return createdMuscleGroupResult.Match(
            muscleGroup => CreatedAtAction(
                nameof(GetMuscleGroup),
                new { muscleGroupId = muscleGroup.Id },
                _mapper.Map<MuscleGroupResponse>(muscleGroup)),
            errors => Problem(errors));
    }
}