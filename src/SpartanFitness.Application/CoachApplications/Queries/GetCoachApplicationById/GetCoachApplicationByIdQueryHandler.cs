using ErrorOr;

using MediatR;

using SpartanFitness.Application.CoachApplications.Common;
using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.CoachApplications.Queries.GetCoachApplicationById;

public class GetCoachApplicationByIdQueryHandler
    : IRequestHandler<GetCoachApplicationByIdQuery, ErrorOr<CoachApplicationResult>>
{
    private readonly ICoachApplicationRepository _coachApplicationRepository;

    public GetCoachApplicationByIdQueryHandler(
        ICoachApplicationRepository coachApplicationRepository)
    {
        _coachApplicationRepository = coachApplicationRepository;
    }

    public async Task<ErrorOr<CoachApplicationResult>> Handle(
        GetCoachApplicationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var id = CoachApplicationId.Create(request.Id);

        if (await _coachApplicationRepository.GetByIdAsync(id) is not CoachApplication application)
        {
            return Errors.CoachApplication.NotFound;
        }

        return new CoachApplicationResult(application);
    }
}