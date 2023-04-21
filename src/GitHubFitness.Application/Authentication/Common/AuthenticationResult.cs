using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.ValueObjects;

namespace GitHubFitness.Application.Authentication.Common;

public record AuthenticationResult(
    User User,
    string Token);