using System.Security.Cryptography;
using System.Text;

using Microsoft.Extensions.Options;

using SpartanFitness.Application.Common.Interfaces.Authentication;
using SpartanFitness.Infrastructure.Services;

namespace SpartanFitness.Infrastructure.Authentication;

public class CoachCreationTokenProvider : ICoachCreationTokenProvider
{
  private readonly CoachSettings _coachSettings;

  public CoachCreationTokenProvider(IOptions<CoachSettings> coachSettings)
  {
    _coachSettings = coachSettings.Value;
  }

  public string GenerateToken(string emailAddress)
  {
    var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_coachSettings.Secret));
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