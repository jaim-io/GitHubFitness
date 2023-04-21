using ErrorOr;

using GitHubFitness.Application.MuscleGroups.Commands.CreateMuscleGroup;
using GitHubFitness.Application.MuscleGroups.Queries.GetMuscleGroupById;
using GitHubFitness.Contracts.MuscleGroups;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.Enums;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubFitness.Api.Controllers;

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