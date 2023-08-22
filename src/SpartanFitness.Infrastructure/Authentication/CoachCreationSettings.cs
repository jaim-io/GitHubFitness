namespace SpartanFitness.Infrastructure.Authentication;

public class CoachCreationSettings
{
  public const string SectionName = "CoachCreationSettings";
  public string Secret { get; init; } = null!;
}