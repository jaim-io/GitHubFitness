using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SpartanFitness.Application.Administrators.Commands;
using SpartanFitness.Application.Administrators.Common;
using SpartanFitness.Contracts.Administrators;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Api.Controllers;

[Route("api/v1/admin")]
public class AdministratorController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public AdministratorController(
        ISender mediator,
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize(Roles = Roles.Administrator)]
    public IActionResult GetAdministrator()
    {
        return Ok();
    }

    [HttpPost]
    // [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> CreateAdministrator(
        CreateAdministratorRequest request)
    {
        var command = _mapper.Map<CreateAdministratorCommand>(request);
        ErrorOr<AdministratorResult> adminResult = await _mediator.Send(command);

        return adminResult.Match(
            adminResult => CreatedAtAction(
                nameof(GetAdministrator), 
                new { id = adminResult.Administrator.Id }, 
                _mapper.Map<AdministratorResponse>(adminResult)),
            errors => Problem(errors));
    }
}