using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.CoachApplications.Queries.GetCoachApplicationById;

public record GetCoachApplicationByIdQuery(
    string Id) : IRequest<ErrorOr<CoachApplication>>;