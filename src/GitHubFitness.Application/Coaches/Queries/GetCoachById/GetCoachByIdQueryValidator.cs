using FluentValidation;

namespace GitHubFitness.Application.Coaches.Queries.GetCoachById;

public class GetCoachByIdQueryValidator
    : AbstractValidator<GetCoachByIdQuery>
{
    public GetCoachByIdQueryValidator()
    {
        RuleFor(x => x.Id)
           .Must(x => Guid.TryParse(x, out _))
           .WithMessage("The coach ID must be a valid GUID");
    }
}