using ErrorOr;

using GitHubFitness.Application.Common.Interfaces.Persistence;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.Common.Errors;
using GitHubFitness.Domain.ValueObjects;

using MediatR;

namespace GitHubFitness.Application.Administrators.Queries.GetAdministratorById;

public class GetAdministratorByIdQueryHandler
    : IRequestHandler<GetAdministratorByIdQuery, ErrorOr<Administrator>>
{
    private readonly IAdministratorRepository _administratorRepository;

    public GetAdministratorByIdQueryHandler(IAdministratorRepository administratorRepository)
    {
        _administratorRepository = administratorRepository;
    }

    public async Task<ErrorOr<Administrator>> Handle(
        GetAdministratorByIdQuery query,
        CancellationToken cancellationToken)
    {
        var adminId = AdministratorId.Create(query.Id);

        if (await _administratorRepository.GetByIdAsync(adminId) is not Administrator admin)
        {
            return Errors.Administrator.NotFound;
        }

        return admin;
    }
}