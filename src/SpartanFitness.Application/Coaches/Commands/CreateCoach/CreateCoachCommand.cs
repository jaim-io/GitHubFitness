using ErrorOr;

using MediatR;

using SpartanFitness.Application.Coaches.Common;
using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Coaches.Commands.CreateCoach;

public record CreateCoachCommand(
  string UserId,
  string Biography,
  string? LinkedInUrl,
  string? WebsiteUrl,
  string? InstagramUrl,
  string? FacebookUrl) : IRequest<ErrorOr<CoachResult>>;