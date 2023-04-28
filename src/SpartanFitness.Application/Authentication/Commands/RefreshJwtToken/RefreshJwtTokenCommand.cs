using ErrorOr;

using MediatR;

using SpartanFitness.Application.Authentication.Common;

namespace SpartanFitness.Application.Authentication.Commands.RefreshJwtToken;

public record RefreshJwtTokenCommand(
  string Token,
  string RefreshTokenId) : IRequest<ErrorOr<AuthenticationResult>>;