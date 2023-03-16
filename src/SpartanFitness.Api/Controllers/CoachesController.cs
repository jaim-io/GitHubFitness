using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SpartanFitness.Application.Coaches.Commands.CreateCoach;
using SpartanFitness.Application.Coaches.Common;
using SpartanFitness.Application.Coaches.Queries.GetCoachById;
using SpartanFitness.Contracts.Coaches;
using SpartanFitness.Domain.Common.Authentication;

namespace SpartanFitness.Api.Controllers;

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
        var request = new GetCoachRequest(coachId);
        var query = _mapper.Map<GetCoachByIdQuery>(request);
        ErrorOr<CoachResult> adminResult = await _mediator.Send(query);

        return adminResult.Match(
            adminResult => Ok(_mapper.Map<CoachResponse>(adminResult)),
            errors => Problem(errors));
    }

    [HttpPost]
    [Authorize(Roles = RoleTypes.Administrator)]
    public async Task<IActionResult> CreateCoach(CreateCoachRequest request)
    {
        var command = _mapper.Map<CreateCoachCommand>(request);
        ErrorOr<CoachResult> coachResult = await _mediator.Send(command);

        return coachResult.Match(
            coachResult => CreatedAtAction(
                nameof(GetCoach),
                new { coachId = coachResult.Coach.Id },
                _mapper.Map<CoachResponse>(coachResult)),
            errors => Problem(errors));
    }
}