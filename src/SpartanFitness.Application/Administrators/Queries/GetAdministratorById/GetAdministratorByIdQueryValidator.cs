using FluentValidation;

namespace SpartanFitness.Application.Administrators.Queries.GetAdministratorById;

public class GetAdministratorByIdQueryValidator
    : AbstractValidator<GetAdministratorByIdQuery>
{
    public GetAdministratorByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("The administrator ID must be a valid GUID");
    }
}