using FluentValidation;

namespace SpartanFitness.Application.Users.Queries.GetUserSaves;

public class GetUserSavesQueryValidator : AbstractValidator<GetUserSavesQuery>
{
  public GetUserSavesQueryValidator()
  {
    RuleFor(x => x.UserId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("The user ID must be a valid GUID");
  }
}