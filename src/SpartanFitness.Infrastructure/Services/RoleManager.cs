using Microsoft.EntityFrameworkCore;

using SpartanFitness.Application.Common.Interfaces.Services;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;
using SpartanFitness.Infrastructure.Persistence;

namespace SpartanFitness.Infrastructure.Services;

public class Rolemanager : IRoleManager
{
    private readonly SpartanFitnessDbContext _dbContext;
    public Rolemanager(SpartanFitnessDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string[]> GetByUserIdAsync(UserId id)
    {
        User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        Coach? coach = await _dbContext.Coaches.FirstOrDefaultAsync(c => c.UserId == id);
        // Admin? admin = await _dbContext.Admins.FirstOrDefaultAsync(a => a.UserId == id);
        var roles = new List<string> {
            user is User
                ? "User" : string.Empty,
            coach is Coach
                ? "Coach" : string.Empty,
            // admin is Admin
            //     ? "Admin" : string.Empty,
        };

        roles.RemoveAll((s) => s == string.Empty);

        return roles.ToArray();
    }
}