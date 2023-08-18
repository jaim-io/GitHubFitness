using ErrorOr;

using MediatR;

using SpartanFitness.Application.Coaches.Common;
using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Coaches.Commands.UpdateCoach;

public record UpdateCoachCommand(
  string CoachId,
  string Biography,
  string? LinkedInUrl,
  string? WebsiteUrl,
  string? InstagramUrl,
  string? FacebookUrl) : IRequest<ErrorOr<CoachResult>>;