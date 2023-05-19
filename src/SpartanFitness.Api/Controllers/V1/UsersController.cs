using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SpartanFitness.Application.Users.Queries.GetUserById;
using SpartanFitness.Contracts.Users;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Enums;

namespace SpartanFitness.Api.Controllers.V1;

[Route("api/v{version:apiVersion}/users")]
public class UsersController : ApiController
{
  private readonly ISender _mediator;
  private readonly IMapper _mapper;

  public UsersController(ISender mediator, IMapper mapper)
  {
    _mediator = mediator;
    _mapper = mapper;
  }

  [HttpGet("{id}")]
  [Authorize(Roles = RoleTypes.Administrator)]
  public async Task<IActionResult> GetUser(string id)
  {
    var query = new GetUserByIdQuery(id);
    ErrorOr<User> result = await _mediator.Send(query);

    return result.Match(
      user => Ok(_mapper.Map<UserResponse>(user)),
      Problem);
  }
}