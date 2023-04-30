using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using ErrorOr;

using MediatR;

using SpartanFitness.Application.Authentication.Common;
using SpartanFitness.Application.Common.Interfaces.Authentication;
using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Authentication.Commands.RefreshJwtToken;

public class RefreshJwtTokenCommandHandler
  : IRequestHandler<RefreshJwtTokenCommand, ErrorOr<AuthenticationResult>>
{
  private readonly IJwtTokenGenerator _jwtTokenGenerator;
  private readonly IRefreshTokenRepository _refreshTokenRepository;
  private readonly IUserRepository _userRepository;
  private readonly IRoleRepository _roleRepository;

  public RefreshJwtTokenCommandHandler(
    IJwtTokenGenerator jwtTokenGenerator,
    IRefreshTokenRepository refreshTokenRepository,
    IUserRepository userRepository,
    IRoleRepository roleRepository)
  {
    _jwtTokenGenerator = jwtTokenGenerator;
    _refreshTokenRepository = refreshTokenRepository;
    _userRepository = userRepository;
    _roleRepository = roleRepository;
  }

  public async Task<ErrorOr<AuthenticationResult>> Handle(RefreshJwtTokenCommand command, CancellationToken cancellationToken)
  {
    if (_jwtTokenGenerator.GetPrincipalFromToken(command.Token) is not ClaimsPrincipal principal)
    {
      // Invalid token
      return Errors.Authentication.InvalidToken;
    }

    // var expiryDateUnix =
    //   long.Parse(principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

    // var expiryDateUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
    //   .AddSeconds(expiryDateUnix);

    // if (expiryDateUtc > DateTime.UtcNow)
    // {
    //   // Token has not expired;
    //   return Errors.Authentication.InvalidToken;
    // }

    var jti = principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

    var refreshTokenId = RefreshTokenId.Create(command.RefreshTokenId);

    if (await _refreshTokenRepository.GetByIdAsync(refreshTokenId) is not RefreshToken storedRefreshToken)
    {
      // Refresh token does not exist;
      return Errors.Authentication.InvalidToken;
    }

    if (DateTime.UtcNow > storedRefreshToken.ExpiryDateTime)
    {
      // Refresh token has expired
      return Errors.Authentication.InvalidToken;
    }

    if (storedRefreshToken.Invalidated)
    {
      // Refresh token is invalidated
      return Errors.Authentication.InvalidToken;
    }

    if (storedRefreshToken.Used)
    {
      // Refresh token has already been used
      // TODO: invalidate all other refresh- and access tokens
      return Errors.Authentication.InvalidToken;
    }

    if (storedRefreshToken.JwtId != jti)
    {
      // Token does not match JWT token (Token pair)
      return Errors.Authentication.InvalidToken;
    }

    storedRefreshToken.Use();
    await _refreshTokenRepository.UpdateAsync(storedRefreshToken);

    var userIdString = principal.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
    var userId = UserId.Create(userIdString);

    if (await _userRepository.GetByIdAsync(userId) is not User user)
    {
      return Errors.User.NotFound;
    }

    var roles = await _roleRepository.GetRolesByUserIdAsync(user.Id);

    var (accessToken, refreshToken) = _jwtTokenGenerator.GenerateTokenPair(user, roles);

    await _refreshTokenRepository.AddAsync(refreshToken);

    return new AuthenticationResult(
      user,
      accessToken,
      refreshToken);
  }
}