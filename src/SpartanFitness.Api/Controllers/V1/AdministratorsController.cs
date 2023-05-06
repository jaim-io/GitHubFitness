using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SpartanFitness.Application.Administrators.Commands;
using SpartanFitness.Application.Administrators.Queries.GetAdministratorById;
using SpartanFitness.Contracts.Administrators;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Enums;

namespace SpartanFitness.Api.Controllers.V1;

[Route("api/v{version:apiVersion}/admins")]
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
                new { adminId = admin.Id.Value }, 
                _mapper.Map<AdministratorResponse>(admin)),
            errors => Problem(errors));
    }
}