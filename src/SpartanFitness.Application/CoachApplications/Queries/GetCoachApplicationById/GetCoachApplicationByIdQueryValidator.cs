using FluentValidation;

namespace SpartanFitness.Application.CoachApplications.Queries.GetCoachApplicationById;

public class GetCoachApplicationByIdQueryValidator
    : AbstractValidator<GetCoachApplicationByIdQuery>
{
    public GetCoachApplicationByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("The string must contain a valid GUID");
    }
}