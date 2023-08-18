using FluentValidation;

namespace SpartanFitness.Application.Coaches.Commands.UpdateCoach;

public class UpdateCoachCommandValidator : AbstractValidator<UpdateCoachCommand>
{
  public UpdateCoachCommandValidator()
  {
    RuleFor(x => x.CoachId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("The coach ID must be a valid GUID");

    RuleFor(x => x.Biography)
      .NotEmpty()
      .MaximumLength(2048);

    RuleFor(x => x.LinkedInUrl)
      .MaximumLength(2048)
      .Must(x => x.StartsWith("https://www.linkedin.com/in/"))
      .WithMessage("LinkedIn url has to start with 'https://www.linkedin.com/in/'.");

    RuleFor(x => x.WebsiteUrl)
      .MaximumLength(2048)
      .Must(x => x.StartsWith("https://"))
      .WithMessage("Website url has to start with 'https://'.");

    RuleFor(x => x.InstagramUrl)
      .MaximumLength(2048)
      .Must(x => x.StartsWith("https://www.instagram.com/"))
      .WithMessage("Website url has to start with 'https://www.instagram.com/'.");

    RuleFor(x => x.FacebookUrl)
      .MaximumLength(2048)
      .Must(x => x.StartsWith("https://www.facebook.com/"))
      .WithMessage("Website url has to start with 'https://www.facebook.com/'.");
  }
}