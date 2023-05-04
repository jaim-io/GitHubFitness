using FluentValidation;

namespace SpartanFitness.Application.MuscleGroups.Commands.CreateMuscleGroup;

public class CreateMuscleGroupCommandValidator 
    : AbstractValidator<CreateMuscleGroupCommand>
{
    public CreateMuscleGroupCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Description)
            .NotEmpty();

        RuleFor(x => x.Image)
            .NotEmpty();
    }
}