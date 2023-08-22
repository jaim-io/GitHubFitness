namespace SpartanFitness.Infrastructure.Authentication;

public class PasswordResetSettings
{
  public const string SectionName = "PasswordResetSettings";
  public string Secret { get; init; } = null!;
  public int ExpiryMinutes { get; init; }
}