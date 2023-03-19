using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SpartanFitness.Application.CoachApplications.Commands.ApproveCoachApplication;
using SpartanFitness.Application.CoachApplications.Commands.CreateCoachApplication;
using SpartanFitness.Application.CoachApplications.Commands.DenyCoachApplication;
using SpartanFitness.Application.CoachApplications.Queries.GetCoachApplicationById;
using SpartanFitness.Contracts.CoachApplications;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Enums;

namespace SpartanFitness.Api.Controllers;

[Route("api/v1/users/{userId}/coach-applications")]
public class CoachApplicationsController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public CoachApplicationsController(
        ISender mediator,
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("{applicationId}")]
    public async Task<IActionResult> GetCoachApplication(string userId, string applicationId)
    {
        var matchesClaim = Authorization.UserIdMatchesClaim(HttpContext, userId);
        var isAdmin = Authorization.IsAdmin(HttpContext);

        if (!matchesClaim && !isAdmin)
        {
            return Unauthorized();
        }

        var request = new GetCoachApplicationByIdRequest(applicationId);
        var query = _mapper.Map<GetCoachApplicationByIdQuery>(request);
        ErrorOr<CoachApplication> applicationResult = await _mediator.Send(query);

        return applicationResult.Match(
            application => Ok(_mapper.Map<CoachApplicationResponse>(application)),
            errors => Problem(errors));
    }

    [HttpPost("apply")]
    public async Task<IActionResult> CreateCoachApplication(
        CreateCoachApplicationRequest request,
        string userId)
    {
        var command = _mapper.Map<CreateCoachApplicationCommand>((request, userId));
        ErrorOr<CoachApplication> createdApplicationResult = await _mediator.Send(command);

        return createdApplicationResult.Match(
            application => CreatedAtAction(
                nameof(GetCoachApplication),
                new { applicationId = application.Id, userId = application.UserId },
                _mapper.Map<CoachApplicationResponse>(application)),
            errors => Problem(errors));
    }

    [HttpPut("{applicationId}/approve")]
    [Authorize(Roles = RoleTypes.Administrator)]
    public async Task<IActionResult> ApproveCoachApplication(
        ApproveCoachApplicationRequest request,
        string userId,
        string applicationId)
    {
        var command = _mapper.Map<ApproveCoachApplicationCommand>((request, userId, applicationId));
        ErrorOr<CoachApplication> applicationResult = await _mediator.Send(command);

        return applicationResult.Match(
            _ => NoContent(),
            errors => Problem(errors));
    }

    [HttpPut("{applicationId}/deny")]
    [Authorize(Roles = RoleTypes.Administrator)]
    public async Task<IActionResult> DenyCoachApplication(
        DenyCoachApplicationRequest request,
        string userId,
        string applicationId)
    {
        var command = _mapper.Map<DenyCoachApplicationCommand>((request, userId, applicationId));
        ErrorOr<CoachApplication> applicationResult = await _mediator.Send(command);

        return applicationResult.Match(
            _ => NoContent(),
            errors => Problem(errors));
    }
}