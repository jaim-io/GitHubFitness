using Microsoft.EntityFrameworkCore;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SpartanFitnessDbContext _dbContext;

    public UserRepository(SpartanFitnessDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(User user)
    {
        _dbContext.AddAsync(user);

        _dbContext.SaveChangesAsync();
    }

    public Task<User?> GetByEmail(string email)
    {
        return _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}