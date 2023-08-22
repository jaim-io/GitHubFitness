using System.Net.Mail;

using FluentValidation;

namespace SpartanFitness.Application.Authentication.Commands.ForgotPassword;

public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
  public ForgotPasswordCommandValidator()
  {
    RuleFor(x => x.EmailAddress)
      .Must(x => MailAddress.TryCreate(x, out _))
      .NotEmpty();
  }
}