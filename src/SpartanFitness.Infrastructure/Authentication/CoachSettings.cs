namespace SpartanFitness.Infrastructure.Authentication;

public class CoachSettings
{
  public const string SectionName = "CoachSettings";
  public string Secret { get; init; } = null!;
}