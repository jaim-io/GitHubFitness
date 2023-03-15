using ErrorOr;

using MediatR;

using SpartanFitness.Application.Administrators.Common;
using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Administrators.Queries.GetAdministratorById;

public class GetAdministratorByIdQueryHandler
    : IRequestHandler<GetAdministratorByIdQuery, ErrorOr<AdministratorResult>>
{
    private readonly IAdministratorRepository _administratorRepository;

    public GetAdministratorByIdQueryHandler(IAdministratorRepository administratorRepository)
    {
        _administratorRepository = administratorRepository;
    }

    public async Task<ErrorOr<AdministratorResult>> Handle(
        GetAdministratorByIdQuery query,
        CancellationToken cancellationToken)
    {
        var adminId = AdministratorId.Create(query.AdminId);

        if (await _administratorRepository.GetByAdminIdAsync(adminId) is not Administrator admin)
        {
            return Errors.Administrator.NotFound;
        }

        return new AdministratorResult(admin);
    }
}