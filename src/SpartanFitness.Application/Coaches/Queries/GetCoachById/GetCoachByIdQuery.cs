using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Coaches.Queries.GetCoachById;

public record GetCoachByIdQuery(
    string Id) : IRequest<ErrorOr<Coach>>;