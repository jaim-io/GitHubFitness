namespace SpartanFitness.Contracts.Coaches;

public record CoachResponse(
  string Id,
  string UserId,
  string FirstName,
  string LastName,
  string ProfileImage,
  string Email,
  string Biography,
  CoachResponseSocialMedia SocialMedia,
  DateTime CreatedDateTime,
  DateTime UpdatedDateTime);

public record CoachResponseSocialMedia(
  string? LinkedInUrl,
  string? WebsiteUrl,
  string? InstagramUrl,
  string? FacebookUrl);