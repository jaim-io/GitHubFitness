namespace SpartanFitness.Contracts.Coaches;

public record CreateCoachRequest(
  string UserId,
  string Biography,
  string? LinkedInUrl,
  string? WebsiteUrl,
  string? InstagramUrl,
  string? FacebookUrl);