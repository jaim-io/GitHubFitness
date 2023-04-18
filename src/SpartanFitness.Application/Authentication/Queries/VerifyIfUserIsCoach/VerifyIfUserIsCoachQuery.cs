using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Authentication.Queries.VerifyIfUserIsCoach;

public record VerifyIfUserIsCoachQuery(
  string UserId,
  string CoachId) : IRequest<ErrorOr<Coach>>;