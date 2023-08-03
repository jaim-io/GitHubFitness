using Microsoft.Extensions.Options;

using SpartanFitness.Application.Common.Interfaces.Services;
using SpartanFitness.Infrastructure.Common.Frontend;

namespace SpartanFitness.Infrastructure.Services;

public class FrontendProvider : IFrontendProvider
{
  private readonly FrontendSettings _frontendSettings;

  public FrontendProvider(IOptions<FrontendSettings> frontendSettings)
  {
    _frontendSettings = frontendSettings.Value;
  }

  public string GetApplicationUrl() => _frontendSettings.ApplicationUrl;
}