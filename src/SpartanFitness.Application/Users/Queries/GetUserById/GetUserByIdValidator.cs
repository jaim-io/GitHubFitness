using FluentValidation;

namespace SpartanFitness.Application.Users.Queries.GetUserById;

public class GetUserByIdValidator : AbstractValidator<GetUserByIdQuery>
{
  public GetUserByIdValidator()
  {
    RuleFor(x => x.Id)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("The user ID must be a valid GUID");
  }
}