using FluentValidation;

namespace SpartanFitness.Application.Exercises.Queries.GetExerciseById;

public class GetExerciseByIdQueryValidator
    : AbstractValidator<GetExerciseByIdQuery>
{
    public GetExerciseByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("The string must contain a valid GUID");
    }
}