namespace SpartanFitness.Contracts.Coaches;

public record UpdateCoachRequest(
  string CoachId,
  string Biography,
  string? LinkedInUrl,
  string? WebsiteUrl,
  string? InstagramUrl,
  string? FacebookUrl);