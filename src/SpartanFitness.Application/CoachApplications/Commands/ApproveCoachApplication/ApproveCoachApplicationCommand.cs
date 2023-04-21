using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.CoachApplications.Commands.ApproveCoachApplication;

public record ApproveCoachApplicationCommand(
    string Id,
    string UserId,
    string Remarks) : IRequest<ErrorOr<CoachApplication>>;