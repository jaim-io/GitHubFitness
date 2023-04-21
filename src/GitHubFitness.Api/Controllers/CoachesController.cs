using ErrorOr;

using GitHubFitness.Application.Coaches.Commands.CreateCoach;
using GitHubFitness.Application.Coaches.Queries.GetCoachById;
using GitHubFitness.Contracts.Coaches;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.Enums;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubFitness.Api.Controllers;

[Route("api/v1/[controller]")]
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
    public async Task<IActionResult> GetCoach(string coachId)
    {
        var query = new GetCoachByIdQuery(coachId);
        ErrorOr<Coach> coachResult = await _mediator.Send(query);

        return coachResult.Match(
            coach => Ok(_mapper.Map<CoachResponse>(coach)),
            errors => Problem(errors));
    }

    [HttpPost]
    [Authorize(Roles = RoleTypes.Administrator)]
    public async Task<IActionResult> CreateCoach(CreateCoachRequest request)
    {
        var command = _mapper.Map<CreateCoachCommand>(request);
        ErrorOr<Coach> createdCoachResult = await _mediator.Send(command);

        return createdCoachResult.Match(
            coach => CreatedAtAction(
                nameof(GetCoach),
                new { coachId = coach.Id },
                _mapper.Map<CoachResponse>(coach)),
            errors => Problem(errors));
    }
}