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
    var uniqueCode = Guid.NewGuid();
    var value = GenerateTokenValue(user.Email, uniqueCode);

    return PasswordResetToken.Create(
      value: value,
      uniqueCode: uniqueCode,
      expires: _dateTimeProvider.UtcNow.AddMinutes(_passwordResetSettings.ExpiryMinutes),
      userId: (UserId)user.Id);
  }

  public bool ValidateToken(PasswordResetToken token, string valueToMatch, User user)
  {
    var controlValue = GenerateTokenValue(user.Email, token.UniqueCode);
    return controlValue == valueToMatch
      && token.UserId == user.Id;
  }

  private string EncodeToHex(byte[] bytes) => BitConverter.ToString(bytes).Replace("-", string.Empty);

  private string GenerateTokenValue(string emailAddress, Guid uniqueCode)
  {
    var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_passwordResetSettings.Secret));
    byte[] bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(emailAddress + uniqueCode));
    string digest = EncodeToHex(bytes);

    var token = $"{digest}{EncodeToHex(Encoding.UTF8.GetBytes(emailAddress))}";
    return token;
  }
}