using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Coaches.Commands.CreateCoach;

public record CreateCoachCommand(
    string UserId) : IRequest<ErrorOr<Coach>>;