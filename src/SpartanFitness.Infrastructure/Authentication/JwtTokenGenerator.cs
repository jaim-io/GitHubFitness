using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using SpartanFitness.Application.Common.Interfaces.Authentication;
using SpartanFitness.Application.Common.Interfaces.Services;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Enums;

namespace SpartanFitness.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
  private readonly IDateTimeProvider _dateTimeProvider;
  private readonly JwtSettings _jwtSettings;
  private readonly TokenValidationParameters _tokenValidationParameters;

  public JwtTokenGenerator(
    IDateTimeProvider dateTimeProvider,
    IOptions<JwtSettings> jwtSettings,
    TokenValidationParameters tokenValidationParameters)
  {
    _dateTimeProvider = dateTimeProvider;
    _tokenValidationParameters = tokenValidationParameters;
    _jwtSettings = jwtSettings.Value;
  }

  public Tuple<string, RefreshToken> GenerateTokenPair(User user, HashSet<Role> roles)
  {
    var jti = Guid.NewGuid().ToString();

    var accessToken = GenerateAccessToken(user, roles, jti);
    var refreshToken = GenerateRefreshToken(user, jti);

    return new Tuple<string, RefreshToken>(accessToken, refreshToken);
  }

  public string GenerateAccessToken(User user, HashSet<Role> roles, string? jti = null)
  {
    var signingCredentials = new SigningCredentials(
      new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
      SecurityAlgorithms.HmacSha256);

    var claims = new List<Claim>
    {
      new Claim(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
      new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
      new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
      new Claim(JwtRegisteredClaimNames.Jti, jti ?? Guid.NewGuid().ToString()),
    };

    foreach (var role in roles)
    {
      claims.Add(new Claim(ClaimTypes.Role, role.Name));
    }

    var securityToken = new JwtSecurityToken(
      issuer: _jwtSettings.Issuer,
      audience: _jwtSettings.Audience,
      expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
      claims: claims,
      signingCredentials: signingCredentials);

    return new JwtSecurityTokenHandler().WriteToken(securityToken);
  }

  public RefreshToken GenerateRefreshToken(User user, string? jti = null)
  {
    return RefreshToken.Create(
      jwtId: jti ?? Guid.NewGuid().ToString(),
      expiryDateTime: DateTime.Now.AddMonths(_jwtSettings.RefreshTokenExpiryMonths),
      userId: user.Id);
  }

  public ClaimsPrincipal? GetPrincipalFromToken(string token)
  {
    var tokenHandler = new JwtSecurityTokenHandler();

    try
    {
      var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
      return !IsJwtWithValidSecurityAlgorithm(validatedToken) ? null : principal;
    }
    catch
    {
      return null;
    }
  }

  private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken) =>
    (validatedToken is JwtSecurityToken jwtSecurityToken) &&
    jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
}