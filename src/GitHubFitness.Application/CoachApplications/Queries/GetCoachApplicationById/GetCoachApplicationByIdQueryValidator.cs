using FluentValidation;

namespace GitHubFitness.Application.CoachApplications.Queries.GetCoachApplicationById;

public class GetCoachApplicationByIdQueryValidator
    : AbstractValidator<GetCoachApplicationByIdQuery>
{
    public GetCoachApplicationByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("The coach-application ID must be a valid GUID");
    }
}