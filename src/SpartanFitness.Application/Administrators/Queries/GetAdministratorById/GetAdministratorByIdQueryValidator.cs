using FluentValidation;

namespace SpartanFitness.Application.Administrators.Queries.GetAdministratorById;

public class GetAdministratorByIdQueryValidator
    : AbstractValidator<GetAdministratorByIdQuery>
{
    public GetAdministratorByIdQueryValidator()
    {
        RuleFor(x => x.AdminId)
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("The string must contain a valid GUID");
    }
}