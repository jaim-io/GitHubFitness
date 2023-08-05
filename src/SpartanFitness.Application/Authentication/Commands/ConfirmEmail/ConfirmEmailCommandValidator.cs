using FluentValidation;

namespace SpartanFitness.Application.Authentication.Commands.ConfirmEmail;

public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
{
  public ConfirmEmailCommandValidator()
  {
    RuleFor(x => x.UserId)
      .Must(x => Guid.TryParse(x, out Guid _))
      .WithMessage("User ID must be a valid GUID");
  }
}