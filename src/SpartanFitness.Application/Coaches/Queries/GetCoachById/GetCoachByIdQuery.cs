using ErrorOr;

using MediatR;

using SpartanFitness.Application.Coaches.Common;

namespace SpartanFitness.Application.Coaches.Queries.GetCoachById;

public record GetCoachByIdQuery(
    string CoachId) : IRequest<ErrorOr<CoachResult>>;