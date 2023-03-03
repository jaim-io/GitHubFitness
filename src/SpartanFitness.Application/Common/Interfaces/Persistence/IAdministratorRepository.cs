using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface IAdministratorRepository
{
    Task<Administrator?> GetByUserIdAsync(UserId id);
    Task AddAsync(Administrator admin);
}