using System.Reflection;

using Mapster;

using MapsterMapper;

namespace SpartanFitness.Api.Common.Mappings;

/// <summary>
/// Static DependencyInjection wich will be injected in /SpartanFitness.Api/Program.cs.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Mapster's mappings to the Presentation layer [Clean Architecture Layers].
    /// </summary>
    public static IServiceCollection AddMappings(
        this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}