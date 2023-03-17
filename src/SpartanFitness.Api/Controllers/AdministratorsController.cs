using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SpartanFitness.Application.Administrators.Commands;
using SpartanFitness.Application.Administrators.Common;
using SpartanFitness.Application.Administrators.Queries.GetAdministratorById;
using SpartanFitness.Contracts.Administrators;
using SpartanFitness.Domain.Enums;

namespace SpartanFitness.Api.Controllers;

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
        var request = new GetAdministratorRequest(adminId);
        var query = _mapper.Map<GetAdministratorByIdQuery>(request);
        ErrorOr<AdministratorResult> adminResult = await _mediator.Send(query);

        return adminResult.Match(
            adminResult => Ok(_mapper.Map<AdministratorResponse>(adminResult)),
            errors => Problem(errors));
    }

    [HttpPost]
    [Authorize(Roles = RoleTypes.Administrator)]
    public async Task<IActionResult> CreateAdministrator(
        CreateAdministratorRequest request)
    {
        var command = _mapper.Map<CreateAdministratorCommand>(request);
        ErrorOr<AdministratorResult> adminResult = await _mediator.Send(command);

        return adminResult.Match(
            adminResult => CreatedAtAction(
                nameof(GetAdministrator), 
                new { adminId = adminResult.Administrator.Id }, 
                _mapper.Map<AdministratorResponse>(adminResult)),
            errors => Problem(errors));
    }
}