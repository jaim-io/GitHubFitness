using FluentValidation;

namespace SpartanFitness.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
  public UpdateUserCommandValidator()
  {
    RuleFor(x => x.Id)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("User id must be a valid GUID.");

    RuleFor(x => x.FirstName)
      .MaximumLength(100)
      .NotEmpty();

    RuleFor(x => x.LastName)
      .MaximumLength(100)
      .NotEmpty();

    RuleFor(x => x.ProfileImage)
      .MaximumLength(2048)
      .NotEmpty();
  }
}