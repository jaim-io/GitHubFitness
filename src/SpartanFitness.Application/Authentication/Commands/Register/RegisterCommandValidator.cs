using System.Net.Mail;

using FluentValidation;

namespace SpartanFitness.Application.Authentication.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
  public RegisterCommandValidator()
  {
    RuleFor(x => x.FirstName)
      .MaximumLength(100)
      .NotEmpty();

    RuleFor(x => x.LastName)
      .MaximumLength(100)
      .NotEmpty();

    RuleFor(x => x.ProfileImage)
      .MaximumLength(2048)
      .NotEmpty();

    RuleFor(x => x.Email)
      .Must(x => MailAddress.TryCreate(x, out _))
      .NotEmpty();

    RuleFor(x => x.Password)
      .MinimumLength(10)
      .MaximumLength(50)
      .Matches(@"[0-9]+")
        .WithMessage("Password must contain a number")
      .Matches(@"[#?!@$%^&*-]+")
        .WithMessage("Password must contain a special character")
      .Matches(@"[a-z]+")
        .WithMessage("Password must contain a lower letter")
      .Matches(@"[A-Z]+")
        .WithMessage("Password must contain an uppercase letter")
      .NotEmpty();
  }
}