using ErrorOr;

using GitHubFitness.Domain.Aggregates;

using MediatR;

namespace GitHubFitness.Application.Authentication.Queries.VerifyIfUserIsCoach;

public record VerifyIfUserIsCoachQuery(
  string UserId,
  string CoachId) : IRequest<ErrorOr<Coach>>;