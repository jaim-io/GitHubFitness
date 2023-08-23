using System.Security.Cryptography;
using System.Text;

using Microsoft.Extensions.Options;

using SpartanFitness.Application.Common.Interfaces.Authentication;
using SpartanFitness.Application.Common.Interfaces.Services;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Authentication;

public class PasswordResetTokenProvider : IPasswordResetTokenProvider
{
  private readonly PasswordResetSettings _passwordResetSettings;
  private readonly IDateTimeProvider _dateTimeProvider;

  public PasswordResetTokenProvider(
    IOptions<PasswordResetSettings> passwordResetSettings,
    IDateTimeProvider dateTimeProvider)
  {
    _dateTimeProvider = dateTimeProvider;
    _passwordResetSettings = passwordResetSettings.Value;
  }

  public PasswordResetToken GenerateToken(User user)
  {
    var value = GenerateTokenValue(user.Email);

    return PasswordResetToken.Create(
      value: value,
      expires: _dateTimeProvider.UtcNow.AddMinutes(_passwordResetSettings.ExpiryMinutes),
      userId: (UserId)user.Id);
  }

  public bool ValidateToken(string tokenValue, User user)
  {
    var valueToMatch = GenerateTokenValue(user.Email);
    return tokenValue == valueToMatch;
  }

  private string EncodeToHex(byte[] bytes) => BitConverter.ToString(bytes).Replace("-", string.Empty);

  private string GenerateTokenValue(string emailAddress)
  {
    var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_passwordResetSettings.Secret));
    byte[] bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(emailAddress));
    string digest = EncodeToHex(bytes);

    var token = $"{digest}{EncodeToHex(Encoding.UTF8.GetBytes(emailAddress))}";
    return token;
  }
}