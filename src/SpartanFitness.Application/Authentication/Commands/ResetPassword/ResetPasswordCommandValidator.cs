using FluentValidation;

namespace SpartanFitness.Application.Authentication.Commands.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
  public ResetPasswordCommandValidator()
  {
    RuleFor(x => x.UserId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("User id must be a valid GUID");

    RuleFor(x => x.Token)
      .NotEmpty();
  }
}