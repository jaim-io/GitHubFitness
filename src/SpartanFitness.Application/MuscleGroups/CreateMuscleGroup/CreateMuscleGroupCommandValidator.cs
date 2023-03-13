using FluentValidation;

namespace SpartanFitness.Application.MuscleGroups.CreateMuscleGroup;

public class CreateMuscleGroupCommandValidator 
    : AbstractValidator<CreateMuscleGroupCommand>
{
    public CreateMuscleGroupCommandValidator()
    {
        RuleFor(x => x.UserId)
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("The string must contain a valid GUID");

        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Description)
            .NotEmpty();

        RuleFor(x => x.Image)
            .NotEmpty();
    }
}