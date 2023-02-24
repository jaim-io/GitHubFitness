using Microsoft.Extensions.DependencyInjection;

namespace SpartanFitness.Api;

/// <summary>
/// Static DependencyInjection wich will be injected in /SpartanFitness.Api/Program.cs.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds the Infrastructure layer [Clean Architecture Layers].
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services;
    }
}