using GitHubFitness.Application.Common.Interfaces.Persistence;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;

namespace GitHubFitness.Infrastructure.Persistence.Repositories;

public class AdministratorRepository : IAdministratorRepository
{
  private readonly GitHubFitnessDbContext _dbContext;

  public AdministratorRepository(GitHubFitnessDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task AddAsync(Administrator admin)
  {
    _dbContext.Add(admin);

    await _dbContext.SaveEntitiesAsync<AdministratorId>();
  }

  public async Task<Administrator?> GetByIdAsync(AdministratorId id)
  {
    return await _dbContext.Administrators.FirstOrDefaultAsync(a => a.Id == id);
  }

  public async Task<Administrator?> GetByUserIdAsync(UserId id)
  {
    return await _dbContext.Administrators.FirstOrDefaultAsync(a => a.UserId == id);
  }
}