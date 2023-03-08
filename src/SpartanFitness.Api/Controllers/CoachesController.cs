using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using SpartanFitness.Application.Coaches.Commands.CreateCoach;
using SpartanFitness.Application.Coaches.Common;
using SpartanFitness.Contracts.Coaches;

namespace SpartanFitness.Api.Controllers;

[Route("api/v1/coaches")]
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

    [HttpGet]
    public IActionResult GetCoach()
    {
        return Ok();
    }

    [HttpPost]
    // [Authorize(Roles = RoleTypes.Administrator)]
    public async Task<IActionResult> CreateCoach(CreateCoachRequest request)
    {
        var command = _mapper.Map<CreateCoachCommand>(request);
        ErrorOr<CoachResult> coachResult = await _mediator.Send(command);

        return coachResult.Match(
            coachResult => CreatedAtAction(
                nameof(GetCoach), 
                new { id = coachResult.Coach.Id }, 
                _mapper.Map<CoachReponse>(coachResult)),
            errors => Problem(errors));
    }
}