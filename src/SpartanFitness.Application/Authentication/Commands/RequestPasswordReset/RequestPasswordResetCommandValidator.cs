using FluentValidation;

namespace SpartanFitness.Application.Authentication.Commands.RequestPasswordReset;

public class RequestPasswordResetCommandValidator : AbstractValidator<RequestPasswordResetCommand>
{
  public RequestPasswordResetCommandValidator()
  {
    RuleFor(x => x.UserId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("User id must be a valid GUID");
  }
}