using GitHubFitness.Api.Common.Errors;
using GitHubFitness.Api.Common.Mappings;

using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;

namespace GitHubFitness.Api;

/// <summary>
/// Static DependencyInjection wich will be injected in /GitHubFitness.Api/Program.cs.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds the Presentation layer [Clean Architecture Layers].
    /// </summary>
    public static IServiceCollection AddPresentation(
        this IServiceCollection services)
    {
        services.AddControllers();
        services.AddMappings();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Swagger API",
                Description = "Spartan Fitness' Swagger API",
            });
        });

        services.AddSingleton<ProblemDetailsFactory, GitHubFitnessProblemDetailsFactory>();

        return services;
    }
}