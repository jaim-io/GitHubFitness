using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface IUserRepository {
    Task<User?> GetByEmailAsync(string email);
    Task<bool> CheckIfExistsAsync(UserId id);
    Task AddAsync(User user);
}