using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.ValueObjects;

namespace GitHubFitness.Application.Common.Interfaces.Persistence;

public interface IAdministratorRepository
{
    Task<Administrator?> GetByIdAsync(AdministratorId id);
    Task<Administrator?> GetByUserIdAsync(UserId id);
    Task AddAsync(Administrator admin);
}