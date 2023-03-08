using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SpartanFitness.Application.CoachApplications.Commands.ApproveCoachApplication;
using SpartanFitness.Application.CoachApplications.Commands.CreateCoachApplication;
using SpartanFitness.Application.CoachApplications.Commands.DenyCoachApplication;
using SpartanFitness.Application.CoachApplications.Common;
using SpartanFitness.Contracts.CoachApplications;
using SpartanFitness.Domain.Common.Authentication;

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
    public IActionResult GetCoachApplication(string userId, string applicationId)
    {
        return Ok();
    }

    [HttpPost("apply")]
    public async Task<IActionResult> CreateCoachApplication(
        CreateCoachApplicationRequest request,
        string userId)
    {
        var command = _mapper.Map<CreateCoachApplicationCommand>((request, userId));
        ErrorOr<CoachApplicationResult> applicationResult = await _mediator.Send(command);

        return applicationResult.Match(
            applicationResult => CreatedAtAction(
                nameof(GetCoachApplication),
                new { applicationId = applicationResult.CoachApplication.Id, userId = applicationResult.CoachApplication.UserId },
                _mapper.Map<CoachApplicationResponse>(applicationResult)),
            errors => Problem(errors));
    }

    [HttpPut("{applicationId}/approve")]
    [Authorize(Roles = RoleTypes.User)]
    public async Task<IActionResult> ApproveCoachApplication(
        ApproveCoachApplicationRequest request,
        string userId,
        string applicationId)
    {
        var command = _mapper.Map<ApproveCoachApplicationCommand>((request, userId, applicationId));
        ErrorOr<CoachApplicationResult> applicationResult = await _mediator.Send(command);

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
        ErrorOr<CoachApplicationResult> applicationResult = await _mediator.Send(command);

        return applicationResult.Match(
            _ => NoContent(),
            errors => Problem(errors));
    }
}