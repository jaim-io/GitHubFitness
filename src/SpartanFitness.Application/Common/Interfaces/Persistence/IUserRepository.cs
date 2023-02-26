using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface IUserRepository {
    Task<User?> GetByEmail(string email);
    void Add(User user);
}