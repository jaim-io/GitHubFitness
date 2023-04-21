using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace GitHubFitness.Domain;

public static class DependencyInjection
{
  public static IServiceCollection AddDomain(this IServiceCollection services)
  {
    services.AddMediatR(typeof(DependencyInjection).Assembly);

    return services;
  }
}