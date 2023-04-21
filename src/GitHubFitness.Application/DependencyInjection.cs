using System.Reflection;

using FluentValidation;

using GitHubFitness.Application.Common.Behaviours;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace GitHubFitness.Api;

/// <summary>
/// Static DependencyInjection wich will be injected in /GitHubFitness.Api/Program.cs.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds the Application layer [Clean Architecture Layers].
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(typeof(DependencyInjection).Assembly);

        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}