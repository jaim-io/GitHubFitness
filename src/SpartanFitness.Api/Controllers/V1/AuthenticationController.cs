using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SpartanFitness.Application.Authentication.Commands.ConfirmEmail;
using SpartanFitness.Application.Authentication.Commands.RefreshJwtToken;
using SpartanFitness.Application.Authentication.Commands.Register;
using SpartanFitness.Application.Authentication.Common;
using SpartanFitness.Application.Authentication.Queries.Login;
using SpartanFitness.Application.Common.Results;
using SpartanFitness.Contracts.Authentication;

namespace SpartanFitness.Api.Controllers.V1;

/// <summary>
/// This controller handles the authentication and registration.
/// </summary>
[Route("api/v{version:apiVersion}/auth")]
[AllowAnonymous]
public class AuthenticationController : ApiController
{
  private readonly ISender _mediator;
  private readonly IMapper _mapper;

  public AuthenticationController(ISender mediator, IMapper mapper)
  {
    _mediator = mediator;
    _mapper = mapper;
  }

  /// <summary>
  /// Registers a new user.
  /// </summary>
  /// <param name="request">Contains a user's information following the RegisterRequest contract.</param>
  /// <returns>The user's information and a JWT token, following the AuthenticationResponse contract.</returns>
  [HttpPost("register")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status409Conflict)]
  public async Task<IActionResult> Register(RegisterRequest request)
  {
    var command = _mapper.Map<RegisterCommand>(request);
    ErrorOr<MessageResult> authResult = await _mediator.Send(command);

    return authResult.Match(
      Ok,
      Problem);
  }

  /// <summary>
  /// Login a user given their email and password.
  /// </summary>
  /// <param name="request">Contains a user's email and password following the LoginRequest contract.</param>
  /// <returns>The user's information and a JWT token, following the AuthenticationResponse contract.</returns>
  [HttpPost("login")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status409Conflict)]
  public async Task<IActionResult> Login(LoginRequest request)
  {
    var query = _mapper.Map<LoginQuery>(request);
    ErrorOr<AuthenticationResult> authResult = await _mediator.Send(query);

    return authResult.Match(
      authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
      errors => Problem(errors));
  }

  [HttpPost("refresh")]
  public async Task<IActionResult> Refresh(RefreshTokenRequest request)
  {
    var command = _mapper.Map<RefreshJwtTokenCommand>(request);
    ErrorOr<AuthenticationResult> authResult = await _mediator.Send(command);

    return authResult.Match(
      authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
      errors => Problem(errors));
  }

  [HttpGet("confirm-email")]
  public async Task<IActionResult> ConfirmEmail(
    [FromQuery(Name = "id")] string userId,
    [FromQuery] string token)
  {
    var command = new ConfirmEmailCommand(UserId: userId, Token: token);

    ErrorOr<MessageResult> result = await _mediator.Send(command);
    return result.Match(
      Ok,
      Problem);
  }
}