using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Authentication.Common;

public record AuthenticationResult(
    User User,
    string Token);