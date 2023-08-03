using System.Security.Cryptography;
using System.Text;

using Microsoft.Extensions.Options;

using SpartanFitness.Application.Common.Interfaces.Authentication;
using SpartanFitness.Infrastructure.Services;

namespace SpartanFitness.Infrastructure.Authentication;

public class EmailConfirmationTokenProvider : IEmailConfirmationTokenProvider
{
  private readonly EmailSettings _emailSettings;

  public EmailConfirmationTokenProvider(IOptions<EmailSettings> emailSettings)
  {
    _emailSettings = emailSettings.Value;
  }

  public string GenerateToken(string emailAddress)
  {
    var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_emailSettings.Secret));
    byte[] bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(emailAddress));
    string digest = EncodeToHex(bytes);

    var token = $"{digest}{EncodeToHex(Encoding.UTF8.GetBytes(emailAddress))}";
    return token;
  }

  public bool ValidateToken(string token, string email)
  {
    var controlToken = GenerateToken(email);
    return controlToken == token;
  }

  private static string EncodeToHex(byte[] bytes)
  {
    return BitConverter.ToString(bytes).Replace("-", string.Empty);
  }
}