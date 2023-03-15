using FluentValidation;

namespace SpartanFitness.Application.Coaches.Queries.GetCoachById;

public class GetCoachByIdQueryValidator
    : AbstractValidator<GetCoachByIdQuery>
{
    public GetCoachByIdQueryValidator()
    {
        RuleFor(x => x.CoachId)
           .Must(x => Guid.TryParse(x, out _))
           .WithMessage("The string must contain a valid GUID");
    }
}