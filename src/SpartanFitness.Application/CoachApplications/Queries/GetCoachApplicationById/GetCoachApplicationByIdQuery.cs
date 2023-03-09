using ErrorOr;

using MediatR;

using SpartanFitness.Application.CoachApplications.Common;

namespace SpartanFitness.Application.CoachApplications.Queries.GetCoachApplicationById;

public record GetCoachApplicationByIdQuery(
    string Id) : IRequest<ErrorOr<CoachApplicationResult>>;