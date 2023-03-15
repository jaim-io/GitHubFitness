using FluentValidation;

namespace SpartanFitness.Application.MuscleGroups.Queries.GetMuscleGroupById;

public class GetMuscleGroupByIdQueryValidator
    : AbstractValidator<GetMuscleGroupByIdQuery>
{
    public GetMuscleGroupByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("The string must contain a valid GUID");
    }
}