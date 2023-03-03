using ErrorOr;

using MediatR;

using SpartanFitness.Application.Coaches.Common;

namespace SpartanFitness.Application.Coaches.Commands.CreateCoach;

public record CreateCoachCommand(
    string UserId) : IRequest<ErrorOr<CoachResult>>;