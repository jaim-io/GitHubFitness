using ErrorOr;

using GitHubFitness.Application.Common.Interfaces.Persistence;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.Common.Errors;
using GitHubFitness.Domain.ValueObjects;

using MediatR;

namespace GitHubFitness.Application.Coaches.Queries.GetCoachById;

public class GetCoachByIdQueryHandler
    : IRequestHandler<GetCoachByIdQuery, ErrorOr<Coach>>
{
    private readonly ICoachRepository _coachRepository;

    public GetCoachByIdQueryHandler(ICoachRepository coachRepository)
    {
        _coachRepository = coachRepository;
    }

    public async Task<ErrorOr<Coach>> Handle(
        GetCoachByIdQuery query,
        CancellationToken cancellationToken)
    {
        var coachId = CoachId.Create(query.Id);

        if (await _coachRepository.GetByIdAsync(coachId) is not Coach coach)
        {
            return Errors.Coach.NotFound;
        }

        return coach;
    }
}