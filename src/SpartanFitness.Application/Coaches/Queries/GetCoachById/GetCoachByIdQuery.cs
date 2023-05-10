using ErrorOr;

using MediatR;

using SpartanFitness.Application.Coaches.Common;
using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Coaches.Queries.GetCoachById;

public record GetCoachByIdQuery(
    string Id) : IRequest<ErrorOr<CoachResult>>;