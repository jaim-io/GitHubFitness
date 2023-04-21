using FluentValidation;

namespace GitHubFitness.Application.Administrators.Commands;

public class CreateAdministratorCommandValidator
    : AbstractValidator<CreateAdministratorCommand>
{
    public CreateAdministratorCommandValidator()
    {
        RuleFor(x => x.UserId)
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("The administrator ID must be a valid GUID");
    }
}