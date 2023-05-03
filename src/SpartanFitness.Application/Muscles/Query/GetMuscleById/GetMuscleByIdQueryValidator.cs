using FluentValidation;

namespace SpartanFitness.Application.Muscles.Query.GetMuscleById;

public class GetMuscleByIdQueryValidator : AbstractValidator<GetMuscleByIdQuery>
{
  public GetMuscleByIdQueryValidator()
  {
    RuleFor(x => x.Id)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("The ID must be a valid GUID");
  }
}