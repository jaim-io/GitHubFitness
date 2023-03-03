using FluentValidation;

namespace SpartanFitness.Application.Administrators.Commands;

public class CreateAdministratorCommandValidator
    : AbstractValidator<CreateAdministratorCommand>
{
    public CreateAdministratorCommandValidator()
    {
        RuleFor(x => x.UserId)
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("The string must contain a valid GUID");
    }
}