using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private static List<User> _users = new();
    public void Add(User user)
    {
        _users.Add(user);
    }

    public User? GetByEmail(string email)
    {
        return _users.SingleOrDefault(u => u.Email == email);
    }
}