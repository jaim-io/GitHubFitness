using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Authentication.Queries.VerifyUserIsCoach;

public record VerifyUserIsCoachQuery(
  string UserId,
  string CoachId) : IRequest<ErrorOr<Coach>>;