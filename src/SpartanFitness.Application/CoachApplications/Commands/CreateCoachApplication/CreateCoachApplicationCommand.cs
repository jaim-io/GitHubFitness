using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.CoachApplications.Commands.CreateCoachApplication;

public record CreateCoachApplicationCommand(
    string UserId,
    string Motivation) : IRequest<ErrorOr<CoachApplication>>;