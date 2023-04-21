using ErrorOr;

using GitHubFitness.Application.Administrators.Commands;
using GitHubFitness.Application.Administrators.Queries.GetAdministratorById;
using GitHubFitness.Contracts.Administrators;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.Enums;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubFitness.Api.Controllers;

[Route("api/v1/admins")]
public class AdministratorsController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public AdministratorsController(
        ISender mediator,
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("{adminId}")]
    [Authorize(Roles = RoleTypes.Administrator)]
    public async Task<IActionResult> GetAdministrator(string adminId)
    {
        var query = new GetAdministratorByIdQuery(adminId);
        ErrorOr<Administrator> adminResult = await _mediator.Send(query);

        return adminResult.Match(
            admin => Ok(_mapper.Map<AdministratorResponse>(admin)),
            errors => Problem(errors));
    }

    [HttpPost]
    [Authorize(Roles = RoleTypes.Administrator)]
    public async Task<IActionResult> CreateAdministrator(
        CreateAdministratorRequest request)
    {
        var command = _mapper.Map<CreateAdministratorCommand>(request);
        ErrorOr<Administrator> createdAdminResult = await _mediator.Send(command);

        return createdAdminResult.Match(
            admin => CreatedAtAction(
                nameof(GetAdministrator), 
                new { adminId = admin.Id }, 
                _mapper.Map<AdministratorResponse>(admin)),
            errors => Problem(errors));
    }
}