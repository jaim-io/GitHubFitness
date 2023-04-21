using ErrorOr;

using GitHubFitness.Application.Common.Interfaces.Persistence;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.Common.Errors;
using GitHubFitness.Domain.ValueObjects;

using MediatR;

namespace GitHubFitness.Application.CoachApplications.Queries.GetCoachApplicationById;

public class GetCoachApplicationByIdQueryHandler
    : IRequestHandler<GetCoachApplicationByIdQuery, ErrorOr<CoachApplication>>
{
    private readonly ICoachApplicationRepository _coachApplicationRepository;

    public GetCoachApplicationByIdQueryHandler(
        ICoachApplicationRepository coachApplicationRepository)
    {
        _coachApplicationRepository = coachApplicationRepository;
    }

    public async Task<ErrorOr<CoachApplication>> Handle(
        GetCoachApplicationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var id = CoachApplicationId.Create(request.Id);

        if (await _coachApplicationRepository.GetByIdAsync(id) is not CoachApplication application)
        {
            return Errors.CoachApplication.NotFound;
        }

        return application;
    }
}