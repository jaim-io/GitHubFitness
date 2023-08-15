using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Domain.ValueObjects;

public class SocialMedia : ValueObject
{
  public string? LinkedInUrl { get; private set; }
  public string? WebsiteUrl { get; private set; }
  public string? InstagramUrl { get; private set; }
  public string? FacebookUrl { get; private set; }

  private SocialMedia(
    string? linkedInUrl,
    string? websiteUrl,
    string? instagramUrl,
    string? facebookUrl)
  {
    LinkedInUrl = linkedInUrl;
    WebsiteUrl = websiteUrl;
    InstagramUrl = instagramUrl;
    FacebookUrl = facebookUrl;
  }

  public static SocialMedia Create(
    string? linkedInUrl,
    string? websiteUrl,
    string? instagramUrl,
    string? facebookUrl)
  {
    return new SocialMedia(
      linkedInUrl: linkedInUrl,
      websiteUrl: websiteUrl,
      instagramUrl: instagramUrl,
      facebookUrl: facebookUrl);
  }

  public override IEnumerable<object> GetEqualityComponents()
  {
    yield return LinkedInUrl ?? string.Empty;
    yield return WebsiteUrl ?? string.Empty;
    yield return InstagramUrl ?? string.Empty;
    yield return FacebookUrl ?? string.Empty;
  }
}