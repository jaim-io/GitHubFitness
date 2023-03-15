using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SpartanFitness.Application.MuscleGroups.Common;
using SpartanFitness.Application.MuscleGroups.CreateMuscleGroup;
using SpartanFitness.Contracts.MuscleGroups;
using SpartanFitness.Domain.Common.Authentication;

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
    public IActionResult GetMuscleGroup(string muscleGroupId)
    {
        return Ok();
    }

    [HttpPost("create")]
    [Authorize(Roles = $"{RoleTypes.Coach}, {RoleTypes.Administrator}")]
    public async Task<IActionResult> CreateMuscleGroup(
      CreateMuscleGroupRequest request)
    {
        var userId = Authorization.GetUserIdFromClaims(HttpContext);
        var command = _mapper.Map<CreateMuscleGroupCommand>((request, userId));
        ErrorOr<MuscleGroupResult> muscleGroupResult = await _mediator.Send(command);

        return muscleGroupResult.Match(
            muscleGroupResult => CreatedAtAction(
                nameof(GetMuscleGroup),
                new { muscleGroupId = muscleGroupResult.MuscleGroup.Id },
                _mapper.Map<MuscleGroupResponse>(muscleGroupResult)),
            errors => Problem(errors));
    }
}