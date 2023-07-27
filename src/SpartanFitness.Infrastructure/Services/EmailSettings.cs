namespace SpartanFitness.Infrastructure.Services;

public class EmailSettings
{
  public const string SectionName = "EmailSettings";
  public string MailGunApiKey { get; init; } = null!;
  public string MailGunDomain { get; init; } = null!;
}